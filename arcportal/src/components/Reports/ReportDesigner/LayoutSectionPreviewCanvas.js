import React, { useEffect, useRef, useState } from 'react';
import * as pdfjsLib from 'pdfjs-dist';
import 'pdfjs-dist/build/pdf.worker';
import { Spinner, SpinnerSize } from '@fluentui/react';
import { generateLayoutSectionPreviewPdf } from '../Functions/PreviewPdfGenerator';
import { translateLayoutSectionToContents } from '../Functions/layoutSectionTranslator';
import { generateLayoutSectionTestData } from '../Functions/layoutSectionTestData';
import { ReportPageDimensions } from '../ReportPageDimensions';

const DEBOUNCE_MS = 300;
const SPINNER_DELAY_MS = 150;
const SPINNER_MIN_DISPLAY_MS = 200;
const { PAGE_WIDTH } = ReportPageDimensions;

/**
 * Builds NoBox preview state keyed by stable field Value ids and grid Name ids.
 * @param {object} section - Live section definition from the editor.
 * @param {Array} contents - Printable ContentsConfig entries from layoutSectionTranslator.
 * @returns {{ fields: Record<string, boolean>, grids: Record<string, boolean> }}
 */
const buildNoBoxPreviewStates = (section, contents = []) => {
    const fields = {};
    const grids = {};

    (section?.Fields || []).forEach((field) => {
        const value = field?.Value ?? field?.value;
        if (field?.NoBox && value) {
            fields[value] = true;
        }
    });

    (section?.Grids || []).forEach((grid, index) => {
        const name = grid?.Name ?? grid?.name;
        if (grid?.NoBox ?? grid?.noBox) {
            grids[`grid-${index}`] = true;
            if (name) {
                grids[name] = true;
            }
        }
    });

    (contents || []).forEach((entry) => {
        if (entry?.Type === 'Table' && entry?.NoBox && entry?.Name) {
            grids[entry.Name] = true;
        }

        [entry?.Column1?.Fields, entry?.Column2?.Fields].forEach((columnFields) => {
            (columnFields || []).forEach((field) => {
                if (field?.NoBox && field?.Value) {
                    fields[field.Value] = true;
                }
            });
        });
    });

    return { fields, grids };
};

/**
 * Renders the layout section designer preview using jsPDF and pdf.js for WYSIWYG output.
 * @param {Object} props - Component props.
 * @param {Object} props.section - Live section definition from the editor.
 * @param {Object|null} props.format - Resolved custom format model.
 * @param {Object|null} props.dataSection - Data section definition.
 * @param {Array<{ Key: string, Value: string }>} props.language - Language catalogue entries.
 * @returns {JSX.Element} Canvas preview element.
 */
const LayoutSectionPreviewCanvas = ({
    section,
    format,
    dataSection,
    language = [],
}) => {
    const canvasRef = useRef(null);
    const [loading, setLoading] = useState(false);
    const debounceRef = useRef(null);
    const spinnerDelayRef = useRef(null);
    const spinnerHideRef = useRef(null);
    const objectUrlRef = useRef(null);
    const renderGenerationRef = useRef(0);
    const spinnerShownAtRef = useRef(0);

    const clearSpinnerTimers = () => {
        if (spinnerDelayRef.current) {
            clearTimeout(spinnerDelayRef.current);
            spinnerDelayRef.current = null;
        }
        if (spinnerHideRef.current) {
            clearTimeout(spinnerHideRef.current);
            spinnerHideRef.current = null;
        }
    };

    const scheduleLoadingEnd = (generation) => {
        const finish = () => {
            if (renderGenerationRef.current === generation) {
                setLoading(false);
            }
        };

        if (!spinnerShownAtRef.current) {
            finish();
            return;
        }

        const elapsed = Date.now() - spinnerShownAtRef.current;
        const remaining = SPINNER_MIN_DISPLAY_MS - elapsed;
        if (remaining > 0) {
            spinnerHideRef.current = setTimeout(finish, remaining);
        } else {
            finish();
        }
    };

    useEffect(() => {
        if (debounceRef.current) {
            clearTimeout(debounceRef.current);
        }

        debounceRef.current = setTimeout(async () => {
            const container = canvasRef.current?.parentElement;
            const generation = ++renderGenerationRef.current;

            clearSpinnerTimers();
            spinnerShownAtRef.current = 0;

            if (!format || !format.Type) {
                container?.removeAttribute('data-preview-text-items');
                container?.removeAttribute('data-preview-nobox-states');
                container?.removeAttribute('data-preview-heading-style');
                setLoading(false);
                return;
            }

            spinnerDelayRef.current = setTimeout(() => {
                if (renderGenerationRef.current === generation) {
                    spinnerShownAtRef.current = Date.now();
                    setLoading(true);
                }
            }, SPINNER_DELAY_MS);

            try {
                const { contents } = translateLayoutSectionToContents({
                    section,
                    format,
                    dataSection,
                });

                if (!contents || contents.length === 0) {
                    container?.removeAttribute('data-preview-text-items');
                    container?.removeAttribute('data-preview-nobox-states');
                    container?.removeAttribute('data-preview-heading-style');
                    return;
                }

                const headingLine = contents[0]?.Heading?.[0];
                if (headingLine?.Text) {
                    container?.setAttribute(
                        'data-preview-heading-style',
                        JSON.stringify({
                            text: headingLine.Text,
                            bold: !!headingLine.Bold,
                            fontSize: headingLine.FontSize ?? 0,
                            left: headingLine.Left ?? 0,
                        })
                    );
                } else {
                    container?.removeAttribute('data-preview-heading-style');
                }

                const testData = generateLayoutSectionTestData(contents, dataSection, section);
                const pdf = await generateLayoutSectionPreviewPdf(contents, testData, language);
                const blob = pdf.output('blob');

                if (objectUrlRef.current) {
                    URL.revokeObjectURL(objectUrlRef.current);
                }

                const pdfUrl = URL.createObjectURL(blob);
                objectUrlRef.current = pdfUrl;

                pdfjsLib.GlobalWorkerOptions.workerSrc = 'pdf.worker.min.mjs';
                const loadedPdf = await pdfjsLib.getDocument(pdfUrl).promise;
                const page = await loadedPdf.getPage(1);
                const baseViewport = page.getViewport({ scale: 1 });
                const scale = PAGE_WIDTH / baseViewport.width;
                const viewport = page.getViewport({ scale });
                const canvas = canvasRef.current;

                if (!canvas) {
                    return;
                }

                const context = canvas.getContext('2d');
                canvas.width = viewport.width;
                canvas.height = viewport.height;
                canvas.style.width = `${PAGE_WIDTH}px`;
                canvas.style.height = `${viewport.height}px`;

                await page.render({
                    canvasContext: context,
                    viewport,
                }).promise;

                const textContent = await page.getTextContent();
                const textItems = textContent.items
                    .filter((item) => item.str && item.str.trim().length > 0)
                    .map((item) => ({
                        text: item.str,
                        x: item.transform[4],
                        y: item.transform[5],
                        fontSize: Math.abs(item.transform[0]),
                        width: item.width,
                    }));

                container?.setAttribute(
                    'data-preview-text-items',
                    JSON.stringify(textItems)
                );
                container?.setAttribute(
                    'data-preview-nobox-states',
                    JSON.stringify(buildNoBoxPreviewStates(section, contents))
                );
            } catch (error) {
                console.error('Layout section preview render failed', error);
                canvasRef.current?.parentElement?.removeAttribute('data-preview-text-items');
                canvasRef.current?.parentElement?.removeAttribute('data-preview-nobox-states');
                canvasRef.current?.parentElement?.removeAttribute('data-preview-heading-style');
            } finally {
                if (spinnerDelayRef.current) {
                    clearTimeout(spinnerDelayRef.current);
                    spinnerDelayRef.current = null;
                }
                scheduleLoadingEnd(generation);
            }
        }, DEBOUNCE_MS);

        return () => {
            if (debounceRef.current) {
                clearTimeout(debounceRef.current);
            }
            clearSpinnerTimers();
        };
    }, [section, format, dataSection, language]);

    useEffect(() => () => {
        if (objectUrlRef.current) {
            URL.revokeObjectURL(objectUrlRef.current);
        }
    }, []);

    return (
        <>
            {loading && (
                <div id="sectionfieldeditor-preview-loading" className="layout-preview-loading-overlay">
                    <Spinner size={SpinnerSize.medium} label="Updating preview..." />
                </div>
            )}
            <canvas
                id="sectionfieldeditor-preview-canvas"
                ref={canvasRef}
                className="preview-canvas"
            />
        </>
    );
};

export default LayoutSectionPreviewCanvas;


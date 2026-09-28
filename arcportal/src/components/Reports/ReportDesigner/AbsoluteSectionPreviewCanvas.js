import React, { useEffect, useRef, useState } from 'react';
import * as pdfjsLib from 'pdfjs-dist';
import 'pdfjs-dist/build/pdf.worker';
import { Spinner, SpinnerSize } from '@fluentui/react';
import { generateAbsoluteSectionPreviewPdf } from '../Functions/PreviewPdfGenerator';
import { ReportPageDimensions } from '../ReportPageDimensions';

const DEBOUNCE_MS = 300;
const { PAGE_WIDTH } = ReportPageDimensions;

/**
 * Renders the absolute section designer preview using jsPDF and pdf.js for WYSIWYG output.
 * @param {Object} props - Component props.
 * @param {Object[]} props.lines - Section line definitions.
 * @param {Object[]} props.images - Section image definitions.
 * @param {Object} props.imageCache - Cached image metadata keyed by file attachment id.
 * @param {Array<{ Key: string, Value: string }>} props.language - Language entries for tag translation.
 * @param {boolean} props.isFooter - Whether the section uses footer inverted Y layout.
 * @param {number} props.lineSpacing - Line spacing in points.
 * @returns {JSX.Element} Canvas preview element.
 */
const AbsoluteSectionPreviewCanvas = ({
    lines,
    images,
    imageCache,
    language,
    isFooter = false,
    lineSpacing = 4,
}) => {
    const canvasRef = useRef(null);
    const [loading, setLoading] = useState(false);
    const debounceRef = useRef(null);
    const objectUrlRef = useRef(null);

    useEffect(() => {
        if (debounceRef.current) {
            clearTimeout(debounceRef.current);
        }

        debounceRef.current = setTimeout(async () => {
            if (!lines || lines.length === 0) {
                canvasRef.current?.parentElement?.removeAttribute('data-preview-text-items');
                return;
            }

            setLoading(true);

            try {
                const sectionDefinition = {
                    Lines: lines,
                    Images: images,
                    LineSpacing: lineSpacing,
                };

                const pdf = await generateAbsoluteSectionPreviewPdf(sectionDefinition, {
                    language,
                    imageCache,
                    isFooter,
                });

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
                    }));

                canvas.parentElement?.setAttribute(
                    'data-preview-text-items',
                    JSON.stringify(textItems)
                );
            } catch (error) {
                console.error('Absolute section preview render failed', error);
                canvasRef.current?.parentElement?.removeAttribute('data-preview-text-items');
            } finally {
                setLoading(false);
            }
        }, DEBOUNCE_MS);

        return () => {
            if (debounceRef.current) {
                clearTimeout(debounceRef.current);
            }
        };
    }, [lines, images, imageCache, language, isFooter, lineSpacing]);

    useEffect(() => () => {
        if (objectUrlRef.current) {
            URL.revokeObjectURL(objectUrlRef.current);
        }
    }, []);

    return (
        <>
            {loading && (
                <div id="absolutesectiondesigner-preview-loading" className="preview-loading">
                    <Spinner size={SpinnerSize.small} label="Updating preview..." />
                </div>
            )}
            <canvas
                id="absolutesectiondesigner-preview-canvas"
                ref={canvasRef}
                className="preview-canvas"
            />
        </>
    );
};

export default AbsoluteSectionPreviewCanvas;

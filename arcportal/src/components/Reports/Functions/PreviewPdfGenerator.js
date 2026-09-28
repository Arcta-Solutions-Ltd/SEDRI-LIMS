import TranslateTag from '../../../Utils/Local/TranslateTag';
import ReportPdf from '../ReportPdf';
import { ReportPageDimensions } from '../ReportPageDimensions';
import { calculateAbsoluteSectionHeight } from './AbsoluteSectionLineLayout';
import { WriteLinesAndSetPosition } from './LineWriter';
import { PlaceImages } from './ReportImagePlacer';
import { SortLines } from './LineSorter';
import { renderContentsPreview } from './layoutSectionRenderer';
import { NewSectionHeight } from './FieldHeights';

const { PAGE_WIDTH, MARGIN_TOP } = ReportPageDimensions;
const PREVIEW_MIN_HEIGHT = 150;
const PREVIEW_BOTTOM_PADDING = 10;
const PREVIEW_LINE_HEIGHT = 28;
const PREVIEW_HEADING_HEIGHT = 30;
const PREVIEW_SECTION_PADDING = 60;

/**
 * Space kept clear below the estimated content so a grid table never paginates in the preview.
 * ReportTableWriter hands jspdf-autotable a bottom margin of the footer height plus 10pt, and the 23pt per
 * line estimate can fall a row short of autotable's real row heights. A table that breaks would continue on
 * a second page, which the single-strip preview does not rasterise, so the grid would vanish from view.
 */
const PREVIEW_TABLE_BOTTOM_RESERVE = 34;

const TAG_PATTERN = /@[^@\s]+@/g;

/**
 * Resolves language tags embedded in preview text.
 * @param {string} text - Raw line text from section configuration.
 * @param {Array<{ Key: string, Value: string }>} language - Language key/value pairs.
 * @returns {string} Translated preview text.
 */
const resolvePreviewText = (text, language) => {
    if (!text) {
        return text;
    }

    if (!language || language.length === 0) {
        return text;
    }

    if (/^@[^@\s]+@$/.test(text)) {
        return TranslateTag(text, language) || text;
    }

    return text.replace(TAG_PATTERN, (tag) => TranslateTag(tag, language) || tag);
};

/**
 * Converts designer image cache entries to the shape expected by PlaceImages.
 * @param {Object[]} images - Section image definitions.
 * @param {Object} imageCache - Cached image metadata keyed by file attachment id.
 * @returns {Promise<Object[]>} Report image payloads for PlaceImages.
 */
const buildPreviewReportImages = async (images, imageCache) => {
    const reportImages = [];

    for (const image of images || []) {
        const cached = imageCache[image.FileAttachmentId];
        if (!cached?.objectUrl && !cached?.dataUrl) {
            continue;
        }

        let value = cached.dataUrl;
        if (!value && cached.objectUrl) {
            const response = await fetch(cached.objectUrl);
            const blob = await response.blob();
            value = await new Promise((resolve, reject) => {
                const reader = new FileReader();
                reader.onload = () => resolve(reader.result);
                reader.onerror = reject;
                reader.readAsDataURL(blob);
            });
        }

        const contentType = cached.contentType || '';
        const format = contentType.includes('png') ? 'PNG' : 'JPEG';

        reportImages.push({
            ...image,
            Value: value,
            Format: format,
        });
    }

    return reportImages;
};

/**
 * Converts section lines to preview-safe text lines for WriteLines.
 * @param {Object[]} lines - Sorted line element definitions.
 * @param {Array<{ Key: string, Value: string }>} language - Language key/value pairs.
 * @returns {Object[]} Lines with Text placeholders and translated tags.
 */
const buildPreviewLines = (lines, language) =>
    lines.map((line) => {
        if (line.Text) {
            return {
                ...line,
                Text: resolvePreviewText(line.Text, language),
            };
        }

        if (line.Field) {
            return {
                ...line,
                Text: `[${line.Field}]`,
                Field: undefined,
            };
        }

        if (line.Calc) {
            return {
                ...line,
                Text: `[${line.Calc}]`,
                Calc: undefined,
            };
        }

        return line;
    });

/**
 * Generates a section preview PDF using the same rendering path as the printed report.
 * @param {Object} sectionDefinition - Section configuration with Lines and Images.
 * @param {Object} [options] - Preview generation options.
 * @param {Array<{ Key: string, Value: string }>} [options.language=[]] - Language entries for tag translation.
 * @param {Object} [options.imageCache={}] - Cached image metadata keyed by file attachment id.
 * @param {boolean} [options.isHeaderOrFooter=true] - Whether default header/footer spacing applies.
 * @param {boolean} [options.isFooter=false] - Whether lines are rendered with inverted Y direction.
 * @param {number} [options.startingY] - Initial baseline Y in points (defaults to MARGIN_TOP for headers).
 * @returns {Promise<ReportPdf>} jsPDF instance sized to the section content.
 */
export const generateAbsoluteSectionPreviewPdf = async (sectionDefinition, options = {}) => {
    const {
        language = [],
        imageCache = {},
        isHeaderOrFooter = true,
        isFooter = false,
        startingY = isFooter ? 0 : MARGIN_TOP,
    } = options;

    const lines = sectionDefinition?.Lines || [];
    const images = sectionDefinition?.Images || [];
    const lineSpacing = sectionDefinition?.LineSpacing ?? (isHeaderOrFooter ? 4 : 2);
    const headerStartingY = isFooter ? 0 : startingY;

    const pageHeight = Math.max(
        calculateAbsoluteSectionHeight(lines, images, {
            startingY: headerStartingY,
            bottomPadding: 0,
        }),
        50
    );

    // jsPDF 3 treats [width, height] arrays with width > height as portrait and swaps
    // dimensions; create A4 then resize to the section strip width × height.
    const pdf = new ReportPdf({ left: 0, right: 0, top: 0, bottom: 0 }, 'a4');
    pdf.internal.pageSize.setWidth(PAGE_WIDTH);
    pdf.internal.pageSize.setHeight(pageHeight);
    pdf.bounds = { width: PAGE_WIDTH, height: pageHeight };

    const reportImages = await buildPreviewReportImages(images, imageCache);
    await PlaceImages(reportImages, pdf);

    const sortedLines = SortLines([...(lines || [])], isFooter);
    const mockData = { Standard: [] };

    if (sortedLines.length > 0) {
        if (isFooter) {
            pdf.setCurrentLine(Math.max(...sortedLines.map((line) => line.Line)));
            pdf.setCurrentLinePosition(pdf.getPageHeight());
        } else {
            pdf.setCurrentLine(Math.min(...sortedLines.map((line) => line.Line)));
            pdf.setCurrentLinePosition(headerStartingY);
        }

        WriteLinesAndSetPosition(
            buildPreviewLines(sortedLines, language),
            pdf,
            mockData,
            language,
            isFooter,
            lineSpacing
        );
    }

    if (isFooter) {
        pdf.horizontalRule();
    } else {
        let maxBottom = pdf.getCurrentLinePosition();

        if (images.length > 0) {
            const maxImageBottom = Math.max(
                ...images.map((image) => (image.Y || 0) + (image.Height || 0))
            );
            maxBottom = Math.max(maxBottom, maxImageBottom);
        }

        pdf.setCurrentLinePosition(maxBottom + pdf.spacing.md);
        pdf.horizontalRule();
    }

    return pdf;
};

/**
 * Translates tag tokens in section headings and field labels for preview output.
 * @param {Object[]} contents - Printable ContentsConfig entries.
 * @param {Array<{ Key: string, Value: string }>} language - Language catalogue entries.
 * @returns {Object[]} Contents with translated heading and label text.
 */
const translateContentsForPreview = (contents, language) =>
    (contents || []).map((section) => {
        const translated = { ...section };

        if (Array.isArray(translated.Heading)) {
            translated.Heading = translated.Heading.map((line) => ({
                ...line,
                Text: resolvePreviewText(line.Text, language),
            }));
        }

        ['Column1', 'Column2', 'Column'].forEach((columnKey) => {
            const column = translated[columnKey];
            if (!column?.Fields) {
                return;
            }

            translated[columnKey] = {
                ...column,
                Fields: column.Fields.map((field) => ({
                    ...field,
                    Label: resolvePreviewText(field.Label, language),
                    Text: field.Text ? resolvePreviewText(field.Text, language) : field.Text,
                })),
            };
        });

        if (Array.isArray(translated.Head)) {
            translated.Head = translated.Head.map((heading) =>
                resolvePreviewText(heading, language)
            );
        }

        return translated;
    });

/**
 * Estimates a minimum preview page height from bound field counts when data-based height is zero.
 * @param {Object} section - Printable ContentsConfig section entry.
 * @param {number} startingY - Initial Y position in points.
 * @returns {number} Minimum page height in points.
 */
const estimatePreviewFieldCountHeight = (section, startingY) => {
    const sectionType = section?.Type ? String(section.Type).toLowerCase() : '';
    const col1Count = section?.Column1?.Fields?.length || 0;
    const col2Count = section?.Column2?.Fields?.length || 0;
    const hasHeading = !!(section?.Heading?.[0]?.Text?.trim());

    let boundRows = 0;
    if (sectionType === 'table') {
        boundRows = 3;
    } else if (sectionType.includes('doublefieldcolumn')) {
        boundRows = Math.max(col1Count, col2Count, 1);
    } else {
        boundRows = Math.max(col1Count + col2Count, 1);
    }

    const headingHeight = hasHeading ? PREVIEW_HEADING_HEIGHT : 0;
    return Math.max(
        PREVIEW_MIN_HEIGHT,
        startingY + headingHeight + boundRows * PREVIEW_LINE_HEIGHT + PREVIEW_SECTION_PADDING
    );
};

/**
 * Generates a layout section preview PDF strip using the same render path as print.
 * @param {Object[]} contents - Translated ContentsConfig entries for one section.
 * @param {Object} testData - Synthetic Standard/Tables preview payload.
 * @param {Array<{ Key: string, Value: string }>} [language=[]] - Language entries for tag translation.
 * @returns {Promise<ReportPdf>} jsPDF instance sized to the section content.
 */
export const generateLayoutSectionPreviewPdf = async (contents, testData, language = []) => {
    const previewContents = translateContentsForPreview(contents, language);
    const startingY = MARGIN_TOP;

    let estimatedHeight = PREVIEW_MIN_HEIGHT;
    previewContents.forEach((section) => {
        const sectionHeight = NewSectionHeight(section, testData, previewContents);
        const dataBasedHeight = sectionHeight.combinedHeight + startingY + PREVIEW_BOTTOM_PADDING + 20
            + PREVIEW_TABLE_BOTTOM_RESERVE;
        const fieldCountHeight = estimatePreviewFieldCountHeight(section, startingY);
        estimatedHeight = Math.max(estimatedHeight, dataBasedHeight, fieldCountHeight);
    });

    const pageHeight = Math.max(estimatedHeight, PREVIEW_MIN_HEIGHT);
    const pdf = new ReportPdf({ left: 20, right: 20, top: 20, bottom: 12 }, 'a4');
    pdf.internal.pageSize.setWidth(PAGE_WIDTH);
    pdf.internal.pageSize.setHeight(pageHeight);
    pdf.bounds = { width: PAGE_WIDTH, height: pageHeight };
    pdf.setCurrentLinePosition(startingY);

    await renderContentsPreview(pdf, previewContents, testData, language, startingY);

    const contentBottom = Math.max(pdf.getCurrentLinePosition(), startingY);
    pdf.setCurrentLinePosition(contentBottom + pdf.spacing.md);
    pdf.horizontalRule();

    return pdf;
};

/**
 * @deprecated Use generateAbsoluteSectionPreviewPdf which returns a ReportPdf instance.
 */
export const generateSectionPreviewPdf = async (
    sectionDefinition,
    width,
    height,
    isHeaderOrFooter,
    imageCache = {}
) => {
    const pdf = await generateAbsoluteSectionPreviewPdf(sectionDefinition, {
        imageCache,
        isHeaderOrFooter,
    });

    return pdf.output('datauristring');
};

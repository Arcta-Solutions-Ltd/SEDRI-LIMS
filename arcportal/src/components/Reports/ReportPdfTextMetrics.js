import ReportPdf from './ReportPdf';
import TranslateTag from '../../Utils/Local/TranslateTag';
import { ReportPageDimensions } from './ReportPageDimensions';
import { REPORT_LINE_FONT_SIZE_DEFAULT } from './ReportFontSizeOptions';

const { PDF_FONT_FAMILY, PDF_DEFAULT_FONT_SIZE, PREVIEW_FONT_FAMILY } = ReportPageDimensions;

const TAG_PATTERN = /@[^@\s]+@/g;

/** @type {CanvasRenderingContext2D|null} Reused canvas context for font ascent measurement. */
let metricsContext = null;

/** @type {ReportPdf|null} Reused instance for text width measurement. */
let metricsPdf = null;

/**
 * Returns a shared canvas 2D context for font metrics.
 * @returns {CanvasRenderingContext2D} Canvas rendering context.
 */
const getMetricsContext = () => {
    if (!metricsContext) {
        const canvas = document.createElement('canvas');
        metricsContext = canvas.getContext('2d');
    }

    return metricsContext;
};

/**
 * Returns a minimal ReportPdf instance for text measurement.
 * @returns {ReportPdf} Shared metrics PDF instance.
 */
const getMetricsPdf = () => {
    if (!metricsPdf) {
        metricsPdf = new ReportPdf({ left: 0, right: 0, top: 0, bottom: 0 }, [10, 10]);
    }

    return metricsPdf;
};

/**
 * Returns the effective font size in points for a line element.
 * When FontSize is omitted or zero the default (11pt) is used; configured values are passed through as-is.
 * @param {number|undefined|null} fontSize - Configured FontSize on the line element.
 * @returns {number} Font size in points used for preview and PDF measurement.
 */
export const getEffectiveFontSize = (fontSize) => {
    if (!fontSize || fontSize === 0) {
        return PDF_DEFAULT_FONT_SIZE ?? REPORT_LINE_FONT_SIZE_DEFAULT;
    }

    return fontSize;
};

/**
 * Resolves language tags embedded in preview text.
 * @param {string} text - Raw line text from section configuration.
 * @param {Array<{ Key: string, Value: string }>} [language] - Language key/value pairs.
 * @returns {string} Translated preview text.
 */
export const resolvePreviewText = (text, language) => {
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
 * Measures text width using the same jsPDF Times font path as printed reports.
 * @param {string} text - Text to measure.
 * @param {number|undefined|null} fontSize - Font size in points.
 * @param {boolean} [bold=false] - Whether bold style is applied.
 * @returns {number} Text width in points.
 */
export const measurePdfTextWidth = (text, fontSize, bold = false) => {
    if (!text) {
        return 0;
    }

    const pdf = getMetricsPdf();
    pdf.setFont(PDF_FONT_FAMILY, bold ? 'bold' : 'normal');
    pdf.setFontSize(getEffectiveFontSize(fontSize));
    return pdf.getTextWidth(text);
};

/**
 * Returns the display text shown for a line in the absolute section preview.
 * @param {{ Text?: string, Field?: string }} line - Line element definition.
 * @param {Array<{ Key: string, Value: string }>} [language] - Language key/value pairs.
 * @returns {string} Preview display text.
 */
export const getPreviewLineDisplayText = (line, language) => {
    if (line.Text) {
        return resolvePreviewText(line.Text, language);
    }

    if (line.Field) {
        return `[${line.Field}]`;
    }

    return '';
};

/**
 * Returns the right edge X coordinate of line text on the page.
 * @param {{ Left?: number, Text?: string, Field?: string, FontSize?: number, Bold?: boolean }} line - Line element.
 * @param {Array<{ Key: string, Value: string }>} [language] - Language key/value pairs.
 * @returns {number} Right edge in points (Left + text width).
 */
export const getLineTextRightEdge = (line, language) => {
    const left = line.Left || 0;
    const displayText = getPreviewLineDisplayText(line, language);
    const textWidth = measurePdfTextWidth(displayText, line.FontSize, line.Bold);
    return left + textWidth;
};

/**
 * Returns whether line text extends past the printable right margin.
 * @param {{ Left?: number, Text?: string, Field?: string, FontSize?: number, Bold?: boolean }} line - Line element.
 * @param {number} pageWidth - Full page width in points.
 * @param {number} marginRight - Right margin in points.
 * @param {Array<{ Key: string, Value: string }>} [language] - Language key/value pairs.
 * @returns {boolean} True when text overflows the right margin.
 */
export const lineTextOverflowsPage = (line, pageWidth, marginRight, language) => {
    const displayText = getPreviewLineDisplayText(line, language);

    if (!displayText) {
        return false;
    }

    return getLineTextRightEdge(line, language) > pageWidth - marginRight;
};

/**
 * Returns font ascent in pt for preview baseline alignment (Times New Roman).
 * @param {number|undefined|null} fontSize - Font size in points.
 * @param {boolean} [bold=false] - Whether bold style is applied.
 * @returns {number} Ascent distance from baseline to top of em-box in points.
 */
export const getPdfFontAscent = (fontSize, bold = false) => {
    const effectiveFontSize = getEffectiveFontSize(fontSize);
    const context = getMetricsContext();
    context.font = `${bold ? 'bold ' : ''}${effectiveFontSize}pt ${PREVIEW_FONT_FAMILY}`;
    const metrics = context.measureText('Hg');

    const canvasAscent = metrics.fontBoundingBoxAscent
        || metrics.actualBoundingBoxAscent
        || 0;

    if (canvasAscent > 0) {
        return canvasAscent;
    }

    return effectiveFontSize * (bold ? 0.85 : 0.71);
};

/**
 * Converts a jsPDF baseline Y coordinate to a CSS top value for absolute-positioned preview lines.
 * @param {number} baselineY - Baseline Y in points (matches LineWriter / jsPDF).
 * @param {number|undefined|null} fontSize - Font size in points.
 * @param {boolean} [bold=false] - Whether bold style is applied.
 * @returns {number} CSS top value in points.
 */
export const baselineYToCssTop = (baselineY, fontSize, bold = false) =>
    baselineY - getPdfFontAscent(fontSize, bold);

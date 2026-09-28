import { SingleDataValue } from './SingleDataValue';
import FormatDate from '../../../Utils/Local/FormatDate';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { ParseColourTag } from './ReportColourParser';
import {
    ABSOLUTE_DEFAULT_FONT_SIZE,
    ABSOLUTE_LINE_SPACING,
    ABSOLUTE_MARGIN_BOTTOM,
} from './AbsoluteSectionLineLayout';

const LINE_SPACING = ABSOLUTE_LINE_SPACING;
const MARGIN_BOTTOM = ABSOLUTE_MARGIN_BOTTOM;
const DEFAULT_FONT_SIZE = ABSOLUTE_DEFAULT_FONT_SIZE;

/**
 * Writes absolute section lines and updates the PDF cursor position.
 * @param {Object[]} lines - Sorted line element definitions.
 * @param {Object} pdf - ReportPdf instance.
 * @param {Object} data - Report data for field resolution.
 * @param {string} language - Language code for tag translation.
 * @param {boolean} [invertYDirection=false] - Whether Y decreases for each new line (footers).
 * @param {number} [lineSpacing=LINE_SPACING] - Spacing between lines in points.
 * @param {number} [marginBottom=MARGIN_BOTTOM] - Trailing margin after the last line.
 * @param {number} [defaultFontSize=DEFAULT_FONT_SIZE] - Default font size when FontSize is omitted.
 */
const writeLinesAndSetPosition = (
    lines,
    pdf,
    data,
    language,
    invertYDirection = false,
    lineSpacing = LINE_SPACING,
    marginBottom = MARGIN_BOTTOM,
    defaultFontSize = DEFAULT_FONT_SIZE
) => {
    const state = writeLines(
        lines,
        pdf,
        data,
        language,
        invertYDirection,
        lineSpacing,
        marginBottom,
        defaultFontSize
    );

    pdf.setCurrentLine(state.currentLine);
    pdf.setCurrentLinePosition(state.currentLinePosition);
};

const renderTextWithColor = (pdf, x, y, text) => {
    const colourTag = ParseColourTag(text);

    if (colourTag) {
        pdf.setTextColor(colourTag.colour);
        text = text.replace(colourTag.text, '');
    }

    pdf.text(x, y, text);
    pdf.setTextColor(0, 0, 0); // Always reset to black
};

/**
 * Writes absolute section lines and returns the updated cursor state.
 * @param {Object[]} lines - Sorted line element definitions.
 * @param {Object} pdf - ReportPdf instance.
 * @param {Object} data - Report data for field resolution.
 * @param {string} language - Language code for tag translation.
 * @param {boolean} [invertYDirection=false] - Whether Y decreases for each new line (footers).
 * @param {number} [lineSpacing=LINE_SPACING] - Spacing between lines in points.
 * @param {number} [marginBottom=MARGIN_BOTTOM] - Trailing margin after the last line.
 * @param {number} [defaultFontSize=DEFAULT_FONT_SIZE] - Default font size when FontSize is omitted.
 * @returns {{ currentLinePosition: number, currentLine: number }} Updated PDF cursor state.
 */
const writeLines = (
    lines,
    pdf,
    data,
    language,
    invertYDirection = false,
    lineSpacing = LINE_SPACING,
    marginBottom = MARGIN_BOTTOM,
    defaultFontSize = DEFAULT_FONT_SIZE
) => {
    let currentLine = pdf.getCurrentLine();
    let currentLinePosition = pdf.getCurrentLinePosition();

    for (const line of lines) {
        if (currentLine !== line.Line) {
            const fontSize =
                !line.FontSize || line.FontSize === 0
                    ? defaultFontSize
                    : line.FontSize;
            pdf.setFontSize(fontSize);
            if (invertYDirection) {
                currentLinePosition -= pdf.getLineHeight('TestString') + lineSpacing;
            } else {
                currentLinePosition += pdf.getLineHeight('TestString') + lineSpacing;
            }
        }
        writeLine(line, data, pdf, currentLinePosition, language);
        
        currentLine = line.Line;
    }
    if (invertYDirection) {
        currentLinePosition -= pdf.getLineHeight('TestString') + lineSpacing;
    } else {
        currentLinePosition += marginBottom;
    }
    return { currentLinePosition, currentLine };
};

/**
 * Writes a single absolute section line element to the PDF at the given Y position.
 * @param {Object} line - Line element definition (Text, Field, Calc, FontSize, Bold, Left).
 * @param {Object} data - Report data for field resolution.
 * @param {Object} pdf - ReportPdf instance.
 * @param {number} currentLinePos - Y position in points.
 * @param {string} language - Language code for tag translation.
 */
const writeLine = (line, data, pdf, currentLinePos, language) => {
    if (line.Bold) {
        pdf.setFont('Times', 'bold');
    } else {
        pdf.setFont('Times', 'normal');
    }

    if (line.FontSize && line.FontSize > 0) {
        pdf.setFontSize(line.FontSize);
    }

    if (line.Text) {
        renderTextWithColor(pdf, line.Left, currentLinePos, line.Text);
    } else if (line.Field) {
        const textToDisplay = SingleDataValue(line.Field, data);
        renderTextWithColor(pdf, line.Left, currentLinePos, textToDisplay);
    } else if (line.Calc) {
        switch (line.Calc.toLowerCase()) {
            case 'printeddate':
                pdf.text(line.Left, currentLinePos, FormatDate(new Date()));
                break;
            case 'pages':
                pdf.text(
                    line.Left,
                    currentLinePos,
                    TranslateTag('@GenPagA@', language) +
                        ' ' +
                        pdf.internal.getCurrentPageInfo().pageNumber +
                        ' ' +
                        TranslateTag('@GenOfA@', language) +
                        ' {total_pages_count_string}'
                );
                break;
            default:
                pdf.text(line.Left, currentLinePos, 'NoDef');
                break;
        }
    }
};

export { writeLinesAndSetPosition as WriteLinesAndSetPosition, writeLine as WriteLine, writeLines as WriteLines };

import { SingleDataValue } from './SingleDataValue';
import FormatDate from '../../../Utils/Local/FormatDate';
import TranslateTag from '../../../Utils/Local/TranslateTag';
import { ParseColourTag } from './ReportColourParser';

/**
 * Renders a list of line elements and updates the report PDF cursor position.
 * @param {Array<{ Line?: number, Left?: number, Text?: string, Field?: string, Calc?: string, FontSize?: number, Bold?: boolean }>} lines - Line elements to render.
 * @param {Object} reportPdf - ReportPdf instance.
 * @param {Object} [data] - Report data for field resolution.
 * @param {number} [totalPages] - Total page count for page-number calculations.
 * @param {string} [language] - Language code for tag translation.
 */
const normalLines = (lines, reportPdf, data, totalPages, language) => {
    const state = normalLinesNoPositionSave(
        lines,
        reportPdf,
        data,
        totalPages,
        language
    );

    reportPdf.setCurrentLine(state.currentLine);
    reportPdf.setCurrentLinePosition(state.currentLinePosition);
};

/**
 * Renders a list of line elements without updating the report PDF cursor position.
 * @param {Array<{ Line?: number, Left?: number, Text?: string, Field?: string, Calc?: string, FontSize?: number, Bold?: boolean }>} lines - Line elements to render.
 * @param {Object} reportPdf - ReportPdf instance.
 * @param {Object} [data] - Report data for field resolution.
 * @param {number} [totalPages] - Total page count for page-number calculations.
 * @param {string} [language] - Language code for tag translation.
 * @returns {{ currentLinePosition: number, currentLine: number }} Updated line position state.
 */
const normalLinesNoPositionSave = (
    lines,
    reportPdf,
    data,
    totalPages,
    language
) => {
    let currentLine = reportPdf.getCurrentLine();
    let currentLinePosition = reportPdf.getCurrentLinePosition();

    for (const line of lines) {
        if (currentLine !== line.Line) {
            const fontSize =
                !line.FontSize || line.FontSize === 0 ? 11 : line.FontSize;
            reportPdf.setFontSize(fontSize);
            currentLinePosition += reportPdf.getLineHeight('Test') + 4;
        }
        singleLine(
            line,
            data,
            reportPdf,
            currentLinePosition,
            totalPages,
            language
        );

        currentLine = line.Line;
    }

    currentLinePosition += 7.5;

    return { currentLinePosition, currentLine };
};

/**
 * Renders a single line element, honouring Bold via setFont before drawing text.
 * @param {{ Line?: number, Left?: number, Text?: string, Field?: string, Calc?: string, FontSize?: number, Bold?: boolean }} line - Line element definition.
 * @param {Object} data - Report data for field resolution.
 * @param {Object} reportPdf - ReportPdf instance.
 * @param {number} currentLinePos - Y position in points.
 * @param {number} totalPages - Total page count for page-number calculations.
 * @param {string} language - Language code for tag translation.
 */
const singleLine = (
    line,
    data,
    reportPdf,
    currentLinePos,
    totalPages,
    language
) => {
    if (line.Bold) {
        reportPdf.setFont('Times', 'bold');
    } else {
        reportPdf.setFont('Times', 'normal');
    }

    if (line.FontSize && line.FontSize > 0) {
        reportPdf.setFontSize(line.FontSize);
    }

    if (line.Text?.trim()) {
        reportPdf.text(line.Left, currentLinePos, line.Text);
    } else {
        if (line.Field) {
            let textToDisplay = SingleDataValue(line.Field, data);

            const colourTag = ParseColourTag(textToDisplay);

            if (colourTag) {
                reportPdf.setTextColor(colourTag.colour);
                textToDisplay = textToDisplay.replace(colourTag.text, '');
            }
            reportPdf.text(line.Left, currentLinePos, textToDisplay);
            reportPdf.setTextColor(0, 0, 0);
        } else {      
            if (line.Calc) {
                if (line.Calc.toLowerCase() === 'printeddate') {
                    reportPdf.text(
                        line.Left,
                        currentLinePos,
                        FormatDate(new Date())
                    );
                }
                if (line.Calc.toLowerCase() === 'pages') {
                    reportPdf.text(
                        line.Left,
                        currentLinePos,
                        TranslateTag('@GenPagA@', language) +
                            ' ' +
                            reportPdf.internal.getCurrentPageInfo().pageNumber +
                            ' ' +
                            TranslateTag('@GenOfA@', language) +
                            ' ' +
                            (totalPages ? totalPages.toString() : '0')
                    );
                }
            } else {
                // Empty heading lines are intentionally skipped; NoDef is reserved for malformed config lines.
                if (line.Text !== undefined && line.Text !== null && String(line.Text).trim() === ''
                    && !line.Field && !line.Calc) {
                    return;
                }
                reportPdf.text(line.Left, currentLinePos, 'NoDef');
            }
        }
    }
};

export {
    normalLines as NormalLines,
    singleLine as SingleLine,
    normalLinesNoPositionSave as NormalLinesNoPositionSave,
};

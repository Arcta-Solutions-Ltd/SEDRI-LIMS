import { ParseColour } from './ReportColourParser';
import autoTable from 'jspdf-autotable';

/**
 * Writes a report grid table to the PDF body using jspdf-autotable.
 * When config.NoBox is true, all cell borders are omitted while header fill colour is retained.
 * @param {object} config - Grid table ContentsConfig from the printable report definition.
 * @param {object} reportPdf - Report PDF state object.
 * @param {Array<string>} rows - Pipe-delimited table row strings.
 */
const writeTableToBody = (config, reportPdf, rows) => {
    if (rows.length > 0) {
        let columns = 0;

        const body = [];
        for (const row of rows) {
            if (row == null || row === '') {
                continue;
            }
            const newRow = row.split('|');
            body.push(newRow);
            columns = newRow.length;
        }

        const colWidths = config.Width
            ? config.Width.split('|').map(Number)
            : [];
        let totalWidth = colWidths.reduce(
            (accumulator, width) => accumulator + width,
            0
        );
        while (colWidths.length < columns) colWidths.push(0);

        if (columns === 1 && totalWidth > 0) {
            colWidths.length = 1;
            colWidths[0] = totalWidth;
        }

        const columnStyles = colWidths.reduce((styles, width, index) => {
            const numericWidth = Number(width);
            if (!isNaN(numericWidth) && numericWidth > 0) {
                styles[index] = { cellWidth: numericWidth };
            }
            return styles;
        }, {});

        const startY =
            reportPdf.getCurrentLinePosition() +
            (config.Head ? reportPdf.spacing.md : reportPdf.spacing.sm);

        const autoTableConfig = {
            head: config.Head && [config.Head],
            startY: startY,
            margin: {
                left: config.Left,
                bottom: reportPdf.getFooterHeight() + 10, // TODO: use reportPdf content margin not 10
                right: config.Right,
                top: reportPdf.getHeaderHeight() + 10, // TODO: use reportPdf content margin not 10
            },
            body: body,
            didDrawCell: (data) => {
                reportPdf.setCurrentLinePosition(
                    data.cursor.y + data.row.height + 3
                );
            },
            columnStyles,
            tableWidth:
                totalWidth ||
                reportPdf.internal.pageSize.width -
                    (reportPdf.getMargins().left +
                        reportPdf.getMargins().right),
        };

        if (config.NoBox) {
            autoTableConfig.theme = 'plain';
            autoTableConfig.styles = { lineWidth: 0 };
            autoTableConfig.bodyStyles = { lineWidth: 0 };
            autoTableConfig.headStyles = {
                lineWidth: 0,
                ...(config.Colour && {
                    fillColor: ParseColour(config.Colour),
                }),
            };
        } else if (config.Colour) {
            autoTableConfig.headStyles = {
                fillColor: ParseColour(config.Colour),
            };
        }

        autoTable(reportPdf, autoTableConfig)
    }
};

export { writeTableToBody as WriteTableToBody };

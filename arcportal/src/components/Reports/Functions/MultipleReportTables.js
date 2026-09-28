import { WriteTableToBody } from './ReportTableWriter';
import { SectionHeading, InlineHeading } from './Headings';

/**
 * Renders a section heading immediately before a grid table is written.
 * Prefers the inline heading on the grid ContentsConfig (grid-only sections) over a
 * deferred heading saved by ReportWriter (mixed sections with linked grids).
 * @param {object} pdf - The report PDF state object.
 * @param {object} config - The grid table ContentsConfig.
 */
const renderSectionHeadingBeforeTable = (pdf, config) => {
    if (config.Heading?.[0]?.Text) {
        SectionHeading(config.Heading, pdf);
        pdf.incrementCurrentLinePositionBy(10);
        return;
    }

    const savedHeading = pdf.getSavedSectionHeading();
    if (savedHeading) {
        SectionHeading(savedHeading, pdf);
        pdf.setSavedSectionHeading(undefined);
        pdf.incrementCurrentLinePositionBy(10);
    }
};

/**
 * Writes one or more report tables for a section's data key.
 * When the table theme is "grid", any deferred or inline section heading is rendered
 * before the table body so grid-only and grid-only-data sections show their HeadingText.
 * @param {object} config - The table ContentsConfig from the printable report definition.
 * @param {object} pdf - The report PDF state object.
 * @param {object} data - The report data payload containing Tables rows.
 */
const multipleReportTables = (config, pdf, data) => {
    const tableData = data.Tables.filter(
        (f) => f.Key.toLowerCase() === config.Data.toLowerCase()
    );

    let currentTable = '';
    let rows = [];

    if (tableData.length > 0 && tableData[0].Rows.length > 0) {
        if (config.Theme === 'grid') {
            renderSectionHeadingBeforeTable(pdf, config);
            WriteTableToBody(config, pdf, tableData[0].Rows);
            return;
        }

        renderSectionHeadingBeforeTable(pdf, config);
        for (const row of tableData[0].Rows) {
            if (row == null || row === '') {
                continue;
            }
            const newRow = row.split('|');
            const currentSection = newRow[0];

            if (currentTable !== currentSection) {
                if (currentTable !== '') {
                    // TODO: have a standard section heading config on the pdf object, use standard page margin etc.
                    InlineHeading(
                        [
                            {
                                Bold: true,
                                FontSize: 11,
                                Left: 24,
                                Line: 1,
                                Text: currentTable,
                            },
                        ],
                        pdf
                    );
                    WriteTableToBody(config, pdf, rows);
                    rows = [];
                }
                currentTable = currentSection;
            }

            rows.push(removeFirstElement(row));
        }

        // TODO: another standard section heading config here to move to jspdf
        InlineHeading(
            [
                {
                    Bold: true,
                    FontSize: 11,
                    Left: 24,
                    Line: 1,
                    Text: currentTable,
                },
            ],
            pdf
        );
        WriteTableToBody(config, pdf, rows);
    }
};

const singleReportTable = (config, doc, data) => {
    const tableData = data.Tables.filter(
        (f) => f.Key.toLowerCase() === config.Data.toLowerCase()
    );

    if (tableData.length > 0) {
        WriteTableToBody(config, doc, tableData[0].Rows);
    }
};

const removeFirstElement = (row) => {
    if (row == null || row === '') {
        return '';
    }
    return row.split('|').slice(1).join('|');
};

export { multipleReportTables as MultipleReportTables, singleReportTable as SingleReportTable };

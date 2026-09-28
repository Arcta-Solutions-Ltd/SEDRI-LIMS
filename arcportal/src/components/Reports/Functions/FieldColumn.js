import { SingleDataValue } from './SingleDataValue';
import { NormalLines } from './NormalLines';
import { SectionHeading } from './Headings';
import {
    RemoveUnusedDoubleFieldData,
    RemoveUnusedSingleFieldData,
} from './FieldRemovalUtils';
import autoTable from 'jspdf-autotable';

/**
 * Builds autotable body rows and per-row field metadata from column field bindings.
 * @param {Array} fields - Column field bindings from the printable section config.
 * @param {object} data - Report data payload.
 * @returns {{ rows: Array<Array<string>>, fieldMeta: Array<{ NoBox: boolean }> }}
 */
const buildFieldTableRows = (fields, data) => {
    const rows = [];
    const fieldMeta = [];

    for (const row of fields || []) {
        const newRow = row.Text
            ? [row.Label, row.Text]
            : [row.Label, SingleDataValue(row.Value, data)];
        rows.push(newRow);
        fieldMeta.push({ NoBox: !!row.NoBox });
    }

    return { rows, fieldMeta };
};

/**
 * Renders label/value rows for one column using jspdf-autotable.
 * Value cells omit borders when the corresponding field has NoBox set.
 * @param {number} margin - Left margin in points.
 * @param {number} tableWidth - Table width in points.
 * @param {Array<Array<string>>} data - Table body rows.
 * @param {object} reportPdf - Report PDF state object.
 * @param {object} options - Column options including labelWidth and fieldMeta.
 * @returns {{ currentLine: number, currentLinePosition: number }}
 */
const fieldColumn = (margin, tableWidth, data, reportPdf, options) => {
    let height = 0;
    const fieldMeta = options.fieldMeta || [];

    const autoTableConfig = {
        startY: reportPdf.getCurrentLinePosition(),
        margin: { left: margin },
        tableWidth: tableWidth,
        theme: 'plain',
        columnStyles: {
            0: { halign: 'left', cellWidth: options.labelWidth },
            1: { halign: 'left', lineWidth: 1 },
        },
        body: data,
        didParseCell: (cellData) => {
            if (
                cellData.section === 'body' &&
                cellData.column.index === 1 &&
                fieldMeta[cellData.row.index]?.NoBox
            ) {
                cellData.cell.styles.lineWidth = 0;
            }
        },
        didDrawCell: (cellData) => {
            height = cellData.cursor.y + cellData.row.height + 3;
        },
    };

    switch (process.env.REACT_APP_LANGUAGE.toUpperCase()) {
        case 'LAO':
            autoTableConfig.styles = {
                font: 'Phetsarath OT',
            };
            break;
        default:
            break;
    }

    autoTable(reportPdf, autoTableConfig);

    reportPdf.setFont('Times');

    return {
        currentLine: reportPdf.getCurrentLine() + data.length,
        currentLinePosition: height,
    };
};

const singleFieldColumn = (section, data, reportPdf, renderOptions = {}) => {
    if (section.Dynamic && !renderOptions.skipDynamicFieldRemoval) {
        RemoveUnusedSingleFieldData(section, data);
    }

    const { rows: tableOne, fieldMeta } = buildFieldTableRows(
        section.Column1.Fields,
        data
    );

    const columnOptions = {
        labelWidth: section.Column1.LabelWidth,
        fieldMeta,
    };
    const positionResult = fieldColumn(
        section.Column1.Left,
        section.Column1.Width,
        tableOne,
        reportPdf,
        columnOptions
    );

    reportPdf.setCurrentLine(positionResult.currentLine);
    reportPdf.setCurrentLinePosition(positionResult.currentLinePosition);
};

const singleFieldColumnWithDynamicHeader = (section, data, reportPdf, renderOptions = {}) => {
    if (section.Heading?.[0]) {
        section.Heading[0].Text = SingleDataValue(
            section.Column1.Fields[0].Value,
            data
        );
    }

    const fieldCopy = [...section.Column1.Fields];
    section.Column1.Fields.shift();
    if (section.Heading?.[0]?.Text) {
        SectionHeading(section.Heading, reportPdf);
    }

    if (section.Column1.Fields.length > 0) {
        singleFieldColumn(section, data, reportPdf, renderOptions);
    }

    section.Column1.Fields = fieldCopy;
};

const doubleFieldColumn = (section, data, reportPdf, renderOptions = {}) => {
    if (section.Dynamic && !renderOptions.skipDynamicFieldRemoval) {
        RemoveUnusedDoubleFieldData(section, data);
    }

    const tableOneResult = buildFieldTableRows(section.Column1?.Fields || [], data);
    let columnOptions = {
        labelWidth: section.Column1?.LabelWidth,
        fieldMeta: tableOneResult.fieldMeta,
    };
    var positionResult1 = fieldColumn(
        section.Column1?.Left,
        section.Column1?.Width,
        tableOneResult.rows,
        reportPdf,
        columnOptions
    );

    let positionResult2 = positionResult1;
    if (section.Column2?.Fields && section.Column2.Fields.length > 0) {
        const tableTwoResult = buildFieldTableRows(section.Column2.Fields, data);
        columnOptions = {
            labelWidth: section.Column2.LabelWidth,
            fieldMeta: tableTwoResult.fieldMeta,
        };
        positionResult2 = fieldColumn(
            section.Column2.Left,
            section.Column2.Width,
            tableTwoResult.rows,
            reportPdf,
            columnOptions
        );
    }

    reportPdf.setCurrentLine(
        positionResult1.currentLinePosition >
            positionResult2.currentLinePosition
            ? positionResult1.currentLine
            : positionResult2.currentLine
    );
    reportPdf.setCurrentLinePosition(
        positionResult1.currentLinePosition >
            positionResult2.currentLinePosition
            ? positionResult1.currentLinePosition
            : positionResult2.currentLinePosition
    );
};

/**
 * Renders a double-column section with a row heading when HeadingText is non-empty.
 * Heading text and geometry come from the merged ContentsConfig.Heading line built by ReportTranslator.
 */
const doubleFieldColumnWithRowHeading = (section, data, reportPdf, renderOptions = {}) => {
    const shouldRender = renderOptions.designerPreview
        ? ((section.Column1?.Fields?.length || 0) + (section.Column2?.Fields?.length || 0)) > 0
        : doesDoubleFieldContainData(section, data);
    if (shouldRender) {
        reportPdf.decrementCurrentLinePositionBy(4);
        if (section.Heading?.[0]?.Text?.trim()) {
            NormalLines(section.Heading, reportPdf, data);
        }
        doubleFieldColumn(section, data, reportPdf, renderOptions);
        reportPdf.setSavedSectionHeading(undefined);
    }
};

/**
 * Renders a single-column section with a row heading when HeadingText is non-empty.
 * Heading text and geometry come from the merged ContentsConfig.Heading line built by ReportTranslator.
 */
const singleFieldColumnWithRowHeading = (section, data, reportPdf, renderOptions = {}) => {
    const shouldRender = renderOptions.designerPreview
        ? (section.Column1?.Fields?.length || 0) > 0
        : doesSingleFieldContainData(section, data);
    if (shouldRender) {
        reportPdf.decrementCurrentLinePositionBy(4);
        if (section.Heading?.[0]?.Text?.trim()) {
            NormalLines(section.Heading, reportPdf, data);
        }
        singleFieldColumn(section, data, reportPdf, renderOptions);
        reportPdf.setSavedSectionHeading(undefined);
    }
};

const singleFieldColumnWithSeparateHeading = (section, data, reportPdf, renderOptions = {}) => {
    const shouldRender = renderOptions.designerPreview
        ? (section.Column1?.Fields?.length || 0) > 0
        : doesSingleFieldContainData(section, data);
    if (shouldRender) {
        if (section.Heading && section.Heading[0].Text) {
            SectionHeading(section.Heading, reportPdf);
        }

        singleFieldColumn(section, data, reportPdf, renderOptions);
        reportPdf.setSavedSectionHeading(undefined);
    }
};

const hasNonEmptyField = (fields, data) =>
    fields.some((row) => SingleDataValue(row.Value, data) !== '');

const doesSingleFieldContainData = (section, data) =>
    hasNonEmptyField(section.Column1.Fields, data);

const doesDoubleFieldContainData = (section, data) =>
    hasNonEmptyField(section.Column1?.Fields || [], data) ||
    hasNonEmptyField(section.Column2?.Fields || [], data);

export {
    doubleFieldColumn as DoubleFieldColumn,
    doubleFieldColumnWithRowHeading as DoubleFieldColumnWithRowHeading,
    singleFieldColumn as SingleFieldColumn,
    singleFieldColumnWithDynamicHeader as SingleFieldColumnWithDynamicHeader,
    singleFieldColumnWithRowHeading as SingleFieldColumnWithRowHeading,
    singleFieldColumnWithSeparateHeading as SingleFieldColumnWithSeparateHeading,
    doesDoubleFieldContainData as DoesDoubleFieldContainData,
};

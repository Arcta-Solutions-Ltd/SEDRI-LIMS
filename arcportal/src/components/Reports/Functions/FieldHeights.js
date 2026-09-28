import { SingleDataValue } from './SingleDataValue';

/**
 * Estimates how much vertical space a section needs, used for page-break decisions.
 * @param {object} section - The printable ContentsConfig entry being measured.
 * @param {object} data - Report data payload, so empty sections measure as zero.
 * @param {Array<object>} contents - Every printable entry, used to find the grids linked to this section.
 * @returns {{singleHeight: number, combinedHeight: number}} The entry's own height, and its height plus up
 * to three linked grid rows beneath it, both in points.
 * @remarks
 * Linked grids that share a layout section row with one another contribute only the height of the tallest
 * grid on that row, because side-by-side areas occupy one row's worth of vertical space between them rather
 * than one each. Measuring them as a sum would break to a new page while there was still room.
 */
const newSectionHeight = (section, data, contents) => {
    let sectionHeight = singleSectionHeight(section, data);
    const singleHeight = sectionHeight;
    let sectionFound = false;
    let currentRowKey = null;
    let currentRowHeight = 0;

    if (section.LinkedSections) {
        let count = Math.min(section.LinkedSections, 3);
        for (const row of contents) {
            if (sectionFound && count > 0) {
                const linkedHeight = singleSectionHeight(row, data);
                const rowKey = row.LayoutRowKey || null;

                if (linkedHeight > 0 && rowKey && rowKey === currentRowKey) {
                    if (linkedHeight > currentRowHeight) {
                        sectionHeight += linkedHeight - currentRowHeight;
                        currentRowHeight = linkedHeight;
                    }
                } else if (linkedHeight > 0) {
                    sectionHeight += linkedHeight;
                    currentRowKey = rowKey;
                    currentRowHeight = linkedHeight;
                    count -= 1;
                }
            }
            if (row.Name === section.Name) {
                sectionFound = true;
            }
        }
    }

    return { singleHeight: singleHeight, combinedHeight: sectionHeight };
};

const singleSectionHeight = (section, data) => {
    let lines = 0;
    const sectionType = section.Type.toLowerCase();

    if (
        sectionType !== 'table' &&
        sectionType !== 'reportimage' &&
        !doesSingleFieldContainData(section, data)
    ) {
        return 0;
    }

    //TODO: Need to check section for images, loop through image.x + image.height and find the max

    switch (sectionType) {
        case 'absolute':
            //TODO: calculate the height of the lines from the lines property, this needs to take into account font size
            break;
        case 'singlefieldcolumn':
            lines = section.Column1?.Fields?.length || 0;
            break;
        case 'singlefieldcolumnwithrowheading':
        case 'singlefieldcolumnwithseparateheading':
            lines = section.Column1?.Fields?.length || 0;
            break;
        case 'dynamicsinglecolumnone':
            if (section.Column1?.Fields?.length > 1) {
                lines = section.Column1.Fields.length - 0.6;
            }
            break;
        case 'doublefieldcolumn':
            const lineOne = section.Column1?.Fields?.length || 0;
            const lineTwo = section.Column2?.Fields?.length || 0;
            lines = Math.max(lineOne, lineTwo);
            break;
        case 'doublefieldcolumnwithrowheading':
            const lOne = section.Column1?.Fields?.length || 0;
            const lTwo = section.Column2?.Fields?.length || 0;
            lines = Math.max(lOne, lTwo);
            break;
        case 'table':
            if (data.Tables && data.Tables.length > 0) {
                const tableData = data.Tables.find(
                    (f) => f.Key.toLowerCase() === section.Data.toLowerCase()
                );
                if (tableData) {
                    lines = tableData.Rows.length;
                    lines += Array.isArray(section.Head) ? 1.4 : 0.2;
                }
            }
            break;
        case 'reportimage':
            if (section.Column1?.Fields) {
                for (const field of section.Column1.Fields) {
                    if (field.Image) {
                        return field.Height;
                    }
                }
            }
            break;
        default:
            break;
    }

    if (section.Heading && section.Heading[0].Text) {
        lines += 1;
    }

    // TODO: need to decide what to return - line*23 or max image height if greater
    return lines * 23;
};

const normalLinesHeight = (lines, data) => {
    let lineNumber = lines[lines.length - 1].Line; //TODO: this isn't going to work if the line numbers are out of order in the json
    return lineNumber * 16;
};

const doesSingleFieldContainData = (section, data) =>
    section?.Column1?.Fields.some(
        (row) => SingleDataValue(row.Value, data) !== ''
    );

export { normalLinesHeight as NormalLinesHeight, newSectionHeight as NewSectionHeight };

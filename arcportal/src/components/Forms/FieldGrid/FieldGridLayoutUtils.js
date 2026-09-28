import EvaluateRules from '../../../Utils/Rules/EvaluateRules';

/**
 * Maps a grid column Width token to its CSS class.
 * @param {string|undefined} width - Width token from field configuration.
 * @returns {string} CSS class name for the column wrapper.
 */
export function mapGridWidthToClass(width) {
    switch (width) {
        case 'narrow':
            return 'fieldgridfield-narrow';
        case 'smaller':
            return 'fieldgridfield-smaller';
        case 'small':
            return 'fieldgridfield-small';
        case 'medium':
            return 'fieldgridfield-medium';
        case 'wide':
            return 'fieldgridfield-wide';
        case 'extrawide':
            return 'fieldgridfield-extrawide';
        default:
            return 'fieldgridfield-content';
    }
}

/**
 * Determines whether a grid column should be visible (mirrors FieldGridField logic).
 * @param {object} column - FieldGridConfig column definition.
 * @param {object} [rowData] - Row data for rule evaluation.
 * @returns {boolean} True when the column should render.
 */
export function isGridColumnVisible(column, rowData) {
    if (column == null) {
        return false;
    }

    if (column.Visible !== undefined && column.Visible === false) {
        return false;
    }

    if ((column.Type || '').toLowerCase() === 'hidden') {
        return false;
    }

    return EvaluateRules('visible', column.Rules, rowData || {});
}

/**
 * Builds header cell descriptors aligned 1:1 with visible data columns.
 * Renders an empty placeholder cell when GridTitle is omitted so column count matches.
 * @param {Array} gridFields - Grid column definitions from field configuration.
 * @param {object} [rowData] - Row data for visibility rule evaluation.
 * @returns {Array<{ id: string, title: string, className: string }>} Header cells to render.
 */
export function buildGridHeaderCells(gridFields, rowData) {
    if (!gridFields || !Array.isArray(gridFields)) {
        return [];
    }

    return gridFields
        .filter((column) => isGridColumnVisible(column, rowData))
        .map((column) => {
            const widthClass = mapGridWidthToClass(column.Width);
            const title =
                column.GridTitle !== undefined && column.GridTitle !== null
                    ? column.GridTitle
                    : '';

            return {
                id: column.Id,
                title,
                className: `${widthClass} fieldgrid-heading`,
            };
        });
}

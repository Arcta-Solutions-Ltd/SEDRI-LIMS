/**
 * Resolves a layout section's stored area row arrangement against its live bindings.
 *
 * JavaScript twin of arc.domain/ExtensionMethods/ReportSectionLayoutRowExtensions.cs. The designer, the
 * preview and the printed report must all agree on which areas sit on which row.
 */

/** Stable type identifiers for the areas a layout section can place on a row. */
export const LAYOUT_AREA_TYPES = {
    FIELDS: 'Fields',
    GRID: 'Grid',
};

/**
 * Normalises a grid or area identifier for id-based comparison.
 * @param {string} id - The identifier to normalise.
 * @returns {string} Trimmed, space-stripped, lowercased id.
 */
export const normaliseAreaId = (id) =>
    String(id ?? '').trim().replace(/\s/g, '').toLowerCase();

/**
 * Determines whether an area type identifier refers to the scalar field block.
 * @param {string} type - Area type identifier, in any casing.
 * @returns {boolean} True when the identifier is the field block type.
 */
export const isFieldsArea = (type) =>
    normaliseAreaId(type) === normaliseAreaId(LAYOUT_AREA_TYPES.FIELDS);

/**
 * Determines whether an area type identifier refers to a bound grid.
 * @param {string} type - Area type identifier, in any casing.
 * @returns {boolean} True when the identifier is the grid type.
 */
export const isGridArea = (type) =>
    normaliseAreaId(type) === normaliseAreaId(LAYOUT_AREA_TYPES.GRID);

/**
 * Builds the DOM id suffix that addresses an area in the section area layout editor.
 * @param {object} area - A layout area.
 * @returns {string} Either "fields" or "grid-{normalisedGridId}".
 */
export const areaDomId = (area) =>
    isFieldsArea(area?.Type ?? area?.type)
        ? 'fields'
        : `grid-${normaliseAreaId(area?.Name ?? area?.name)}`;

/**
 * Reads an area's requested share of its row.
 * @param {object} area - A layout area.
 * @returns {number} The stored percentage, or zero when the area takes an equal share.
 */
const areaWidthPercent = (area) => {
    const percent = Number(area?.WidthPercent ?? area?.widthPercent ?? 0);
    return percent > 0 ? percent : 0;
};

/**
 * Resolves the arrangement a section should render with.
 * @param {Array<object>} rows - The arrangement stored on the section, which may be stale or incomplete.
 * @param {Array<object>} grids - The section's grid bindings, in format grid position order.
 * @param {boolean} hasFieldBlock - Whether the section renders a scalar field block at all.
 * @returns {Array<{Areas: Array<{Type: string, Name: string|null, WidthPercent: number}>}>} One row per
 * rendered row, covering every current area exactly once. An empty stored arrangement resolves to the legacy
 * stacked order: the field block, then each grid on its own row.
 */
export const resolveLayoutRows = (rows, grids, hasFieldBlock) => {
    const boundGridIds = (grids || [])
        .map((grid) => grid?.Name ?? grid?.name)
        .filter((name) => String(name ?? '').trim().length > 0);

    const resolved = [];
    const placedGridIds = [];
    let fieldBlockPlaced = false;

    for (const row of rows || []) {
        const areas = [];

        for (const area of row?.Areas ?? row?.areas ?? []) {
            const type = area?.Type ?? area?.type;

            if (isFieldsArea(type)) {
                if (!hasFieldBlock || fieldBlockPlaced) {
                    continue;
                }

                fieldBlockPlaced = true;
                areas.push({
                    Type: LAYOUT_AREA_TYPES.FIELDS,
                    Name: null,
                    WidthPercent: areaWidthPercent(area),
                });
                continue;
            }

            if (!isGridArea(type)) {
                continue;
            }

            const areaGridId = normaliseAreaId(area?.Name ?? area?.name);
            const boundGridId = boundGridIds.find((id) => normaliseAreaId(id) === areaGridId);

            if (!boundGridId || placedGridIds.some((id) => normaliseAreaId(id) === areaGridId)) {
                continue;
            }

            placedGridIds.push(boundGridId);
            areas.push({
                Type: LAYOUT_AREA_TYPES.GRID,
                Name: boundGridId,
                WidthPercent: areaWidthPercent(area),
            });
        }

        if (areas.length > 0) {
            resolved.push({ Areas: areas });
        }
    }

    if (hasFieldBlock && !fieldBlockPlaced) {
        resolved.unshift({
            Areas: [{ Type: LAYOUT_AREA_TYPES.FIELDS, Name: null, WidthPercent: 0 }],
        });
    }

    boundGridIds
        .filter((id) => !placedGridIds.some((placed) => normaliseAreaId(placed) === normaliseAreaId(id)))
        .forEach((id) => {
            resolved.push({
                Areas: [{ Type: LAYOUT_AREA_TYPES.GRID, Name: id, WidthPercent: 0 }],
            });
        });

    return resolved;
};

/**
 * Determines whether an arrangement actually places two or more areas together on any row.
 * @param {Array<object>} rows - A resolved arrangement.
 * @returns {boolean} True when at least one row holds more than one area.
 */
export const hasSideBySideRow = (rows) =>
    (rows || []).some((row) => (row?.Areas?.length ?? 0) > 1);

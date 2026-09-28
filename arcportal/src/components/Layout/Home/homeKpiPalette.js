/**
 * KPI tile colour swatches: stable string ids (persisted in HomeDashboard JSON), never translated labels.
 * Maps to hex for display; ids are not listitem database ids.
 */

export const HOME_KPI_COLOR_CELLS = [
    { id: 'themePrimary', label: 'Blue', color: '#0078d4' },
    { id: 'themeDark', label: 'Dark blue', color: '#004578' },
    { id: 'green', label: 'Green', color: '#107c10' },
    { id: 'teal', label: 'Teal', color: '#038387' },
    { id: 'purple', label: 'Purple', color: '#5c2d91' },
    { id: 'magenta', label: 'Magenta', color: '#881798' },
    { id: 'orange', label: 'Orange', color: '#ca5010' },
    { id: 'red', label: 'Red', color: '#a4262c' },
    { id: 'neutralDark', label: 'Charcoal', color: '#323130' },
    { id: 'neutralSecondary', label: 'Grey', color: '#605e5c' },
];

export const DEFAULT_KPI_COLOR_SWATCH_ID = 'themePrimary';

export const KPI_FONT_SIZE_MIN = 20;
export const KPI_FONT_SIZE_MAX = 96;
export const DEFAULT_KPI_FONT_SIZE_PX = 40;

const ALLOWED_SWATCH_IDS = new Set(HOME_KPI_COLOR_CELLS.map((c) => c.id));

/**
 * @param {string|undefined} id
 * @returns {string} Hex colour for CSS `color`.
 */
export function resolveKpiSwatchHex(id) {
    const cell = HOME_KPI_COLOR_CELLS.find((c) => c.id === id);
    return cell?.color ?? HOME_KPI_COLOR_CELLS[0].color;
}

/**
 * Normalizes KPI-specific persisted fields on a home section.
 * @param {object} section
 * @returns {object}
 */
export function normalizeKpiSection(section) {
    if (!section || section.visualization !== 'kpi') {
        return section;
    }
    let swatchId =
        typeof section.kpiColorSwatchId === 'string' && ALLOWED_SWATCH_IDS.has(section.kpiColorSwatchId)
            ? section.kpiColorSwatchId
            : DEFAULT_KPI_COLOR_SWATCH_ID;
    let fontPx = parseInt(String(section.kpiFontSizePx != null ? section.kpiFontSizePx : DEFAULT_KPI_FONT_SIZE_PX), 10);
    if (!Number.isFinite(fontPx)) {
        fontPx = DEFAULT_KPI_FONT_SIZE_PX;
    }
    fontPx = Math.min(KPI_FONT_SIZE_MAX, Math.max(KPI_FONT_SIZE_MIN, fontPx));
    const bold = section.kpiBold === undefined ? true : Boolean(section.kpiBold);
    return {
        ...section,
        kpiColorSwatchId: swatchId,
        kpiFontSizePx: fontPx,
        kpiBold: bold,
    };
}

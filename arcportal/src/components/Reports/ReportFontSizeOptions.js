/** Minimum allowed report line font size in points. */
export const REPORT_LINE_FONT_SIZE_MIN = 6;

/** Maximum allowed report line font size in points. */
export const REPORT_LINE_FONT_SIZE_MAX = 20;

/** Default font size when FontSize is omitted or 0 — matches LineWriter / PDF_DEFAULT_FONT_SIZE. */
export const REPORT_LINE_FONT_SIZE_DEFAULT = 11;

/** Dropdown presets for the Absolute Section Designer font size control. */
export const REPORT_LINE_FONT_SIZE_DROPDOWN_OPTIONS = [
    { key: 6, text: '6pt (Smallest)' },
    { key: 7, text: '7pt' },
    { key: 8, text: '8pt' },
    { key: 9, text: '9pt' },
    { key: 10, text: '10pt' },
    { key: 11, text: '11pt (Default)' },
    { key: 12, text: '12pt (Normal)' },
    { key: 16, text: '16pt (Large)' },
    { key: 18, text: '18pt (Larger)' },
    { key: 20, text: '20pt (Largest)' },
];

/**
 * Clamps a configured line font size to the allowed range.
 * Zero or omitted values are left unchanged (renderer applies the default at print time).
 * @param {number|undefined|null} fontSize - Configured FontSize on a line element.
 * @returns {number} Clamped font size, or 0 when absent.
 */
export function clampReportLineFontSize(fontSize) {
    const numeric = parseInt(String(fontSize ?? 0), 10);

    if (!Number.isFinite(numeric) || numeric === 0) {
        return 0;
    }

    return Math.min(REPORT_LINE_FONT_SIZE_MAX, Math.max(REPORT_LINE_FONT_SIZE_MIN, numeric));
}

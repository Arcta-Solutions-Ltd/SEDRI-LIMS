/**
 * Shared rules for which fields appear in GeneralViewer / FieldViewer.
 * Keeps section titles (e.g. Attachments) in sync with visible upload galleries.
 */

/**
 * @param {unknown} v
 * @returns {boolean}
 */
export function hasValidUploadValue(v) {
    if (v === undefined || v === null || v === '') return false;
    if (Number(v) > 0) return true;
    const csv = String(v);
    const ids = csv.split(',').map((s) => parseInt(s.trim(), 10)).filter((n) => Number.isFinite(n) && n > 0);
    return ids.length > 0;
}

/**
 * @param {{ Type?: string, Value?: unknown }} field
 * @returns {boolean}
 */
export function fieldIsVisibleInViewer(field) {
    if (field.Type === 'upload') {
        return hasValidUploadValue(field.Value);
    }
    return field.Value !== undefined && field.Value !== null && field.Value !== '';
}

/**
 * @param {{ Fields?: unknown[], SubSections?: unknown[] }} subsection
 * @returns {boolean}
 */
export function subsectionHasVisibleContent(subsection) {
    const fields = subsection.Fields ?? [];
    return fields.some(fieldIsVisibleInViewer);
}

/**
 * @param {{ Fields?: unknown[], SubSections?: unknown[] }} section
 * @returns {boolean}
 */
export function sectionHasVisibleContent(section) {
    const fields = section.Fields ?? [];
    if (fields.some(fieldIsVisibleInViewer)) return true;
    const subs = section.SubSections ?? [];
    return subs.some(subsectionHasVisibleContent);
}

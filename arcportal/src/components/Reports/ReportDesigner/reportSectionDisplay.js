import TranslateTag from '../../../Utils/Local/TranslateTag';

/**
 * Returns the user-facing title for a report section in the designer list.
 * Translates @Tag@ Description tokens; falls back to Name for unresolved or string sections.
 * @param {object|string} section - Section definition object or unresolved config name string.
 * @param {Array<{Key: string, Value: string}>} language - Login language catalogue.
 * @returns {string} Translated description, or the config name when no description is available.
 */
export function getSectionDisplayTitle(section, language) {
    if (typeof section === 'string') {
        return section;
    }

    const raw = section.Description || section.Name || '';
    const translated = TranslateTag(raw, language) || raw;
    return translated || section.Name || '';
}

/**
 * Returns the secondary metadata line for a section list row (counts only).
 * @param {object} section - Section definition object.
 * @returns {string} e.g. "7 fields, 0 grids" or "3 lines".
 */
export function getSectionMetaLabel(section) {
    if (section.Lines) {
        return `${section.Lines.length} lines`;
    }

    const fieldCount = section.Fields?.length || 0;
    const gridCount = section.Grids?.length || 0;
    return `${fieldCount} fields, ${gridCount} grids`;
}

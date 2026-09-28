/**
 * Normalizes crafted page Contents to the key/value array shape crafted components expect.
 * Record copies from AddBlankCraftedStructureForPagesWithoutData arrive as plain objects; the form
 * shell discards bare objects, so they must be converted before render (same contract as
 * TableEntryCraftedExtensions.ToCraftedContents on the backend).
 *
 * @param {*} contents - Crafted page contents from the form payload.
 * @returns {Array<{Key?: string, key?: string, Value?: *, value?: *}>} Key/value entries for the page.
 */
const NormalizeCraftedPageContents = (contents) => {
    if (contents == null) {
        return [];
    }

    if (Array.isArray(contents)) {
        return contents;
    }

    if (typeof contents === 'string') {
        try {
            return NormalizeCraftedPageContents(JSON.parse(contents));
        } catch {
            return [];
        }
    }

    if (typeof contents === 'object') {
        return Object.entries(contents).map(([key, value]) => ({
            Key: key,
            key: key,
            Value: value,
            value: value,
        }));
    }

    return [];
};

export default NormalizeCraftedPageContents;

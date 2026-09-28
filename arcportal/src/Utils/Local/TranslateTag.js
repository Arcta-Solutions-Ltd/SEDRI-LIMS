/**
 * Translates a key to its corresponding value based on the provided language array.
 * 
 * @param {string} key - The key to translate.
 * @param {Array<Object>} language - The array of language objects containing key-value pairs.
 * @returns {string} The translated value, or an empty string if not found.
 */
const TranslateTag = (key, language) => {
    if (!language) {
        return "";
    }

    const entry = language.find((l) => (l.Key ?? l.key) === key);
    return entry ? (entry.Value ?? entry.value ?? '') : '';
}

export default TranslateTag;

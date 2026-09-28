/**
 * Returns a new object with the specified keys excluded.
 * @param {Object} obj - The source object.
 * @param {Array<string>} keysToExclude - Keys to exclude from the result.
 * @returns {Object} A new object without the excluded keys.
 */
const ExcludeKeysFromObject = (obj, keysToExclude) => {
    if (obj == null || !Array.isArray(keysToExclude)) return obj ?? {};
    const excludeSet = new Set(keysToExclude.map(k => String(k)));
    return Object.fromEntries(
        Object.entries(obj).filter(([k]) => !excludeSet.has(k))
    );
};

export default ExcludeKeysFromObject;

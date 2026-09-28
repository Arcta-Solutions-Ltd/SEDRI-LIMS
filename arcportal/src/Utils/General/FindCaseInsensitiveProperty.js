
/**
 * Finds a property in an object without considering the case sensitivity of the property name.
 * 
 * This function searches through the keys of the provided object and returns the first key that matches the specified property name,
 * regardless of the case sensitivity of the property name.
 * 
 * @param {Object} obj - The object in which to search for the property.
 * @param {string} propName - The name of the property to find, case insensitive.
 * @returns {string|undefined} - The matched property name in the object's keys, or undefined if no match is found.
 */
const FindCaseInsensitiveProperty = (obj, propName) => {
    if (obj !== undefined) {
        const lowerCasePropName = propName.toLowerCase();
        return Object.keys(obj).find(key => key.toLowerCase() === lowerCasePropName);
    } else {
        return undefined;
    }
};

/**
 * Finds the value of a property in an object, ignoring case.
 *
 * @param {object} obj - The object to search in.
 * @param {string} targetKey - The key to search for (case insensitive).
 * @returns {any} The value of the matching property, or undefined if not found.
 */
function GetPropertyValueIgnoreCase(obj, targetKey) {
    if (obj !== undefined) {
        const key = Object.keys(obj).find((k) => k.toLowerCase() === targetKey.toLowerCase());
        return key ? obj[key] : undefined; // Return the value directly
    } else {
        return undefined;
    }
}

export default FindCaseInsensitiveProperty;

export {GetPropertyValueIgnoreCase}







/**
 * Normalizes an isolate test config name to the canonical form name used in form definitions.
 * Handles both "testname" and "testnameform" formats; appends "form" if missing.
 * Matching is case-insensitive and uses config ids, not translated titles.
 *
 * @param {string|null|undefined} testName - Form config name from lab config or selection Key
 * @returns {string} Normalized lower-case form name (e.g. "betalactamasetestform")
 */
const normalizeCultureTestFormName = (testName) => {
    if (testName == null || testName === '') {
        return '';
    }

    const lower = String(testName).toLowerCase();
    if (lower.endsWith('form')) {
        return lower;
    }

    return `${lower}form`;
};

export default normalizeCultureTestFormName;

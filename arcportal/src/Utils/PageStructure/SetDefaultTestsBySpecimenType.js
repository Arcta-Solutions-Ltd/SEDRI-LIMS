import normalizeCultureTestFormName from '../Specimen/normalizeCultureTestFormName';

/**
 * Sets the Display and Allowed flags on a test selection page from the specimen type options and defaults.
 * Options control which tests are visible; defaults control which are pre-selected.
 * @param {Array} contents - Crafted pages from the form response.
 * @param {Object|undefined} options - Laboratory options for the specimen type, with a Values array of config names.
 * @param {Object|undefined} defaults - Laboratory defaults for the specimen type, with a Values array of config names.
 * @param {string} pageName - Name of the crafted page to update.
 * @param {boolean} ignoreDefaults - When true, existing Allowed values are preserved.
 * @returns {void}
 */
const SetDefaultTestsBySpecimenType = (contents, options, defaults, pageName, ignoreDefaults) => {

    const elementToChange = contents.findIndex((page) => { return page.Name === pageName})

    if (elementToChange > -1) {
        for (const itemToClear of contents[elementToChange].Contents) {
            itemToClear.Allowed = ignoreDefaults ? itemToClear.Allowed : 'No';
            itemToClear.Display = options?.Values.length > 0 && elementToChange > -1 ? 'No' : 'Yes';
        }
    }

    if (! ignoreDefaults) {
        if (defaults?.Values.length > 0 && elementToChange > -1) {
            for (const item of defaults.Values) { 
                const itemToChange = contents[elementToChange].Contents.findIndex((i) => { return i.Key === item });
                if (itemToChange > -1) {
                    contents[elementToChange].Contents[itemToChange].Allowed = "Yes";
                }
            }
        }
    }

    if (options?.Values.length > 0 && elementToChange > -1) {
        for (const item of options.Values) { 
            const itemToChange = contents[elementToChange].Contents.findIndex((i) => { return i.Key === item });
            if (itemToChange > -1) {
                contents[elementToChange].Contents[itemToChange].Display = "Yes";
            }
        }
    }
}

/**
 * Reads the allowed isolate test config names for a culture type. The map is keyed by the culture
 * type list item id, which may arrive as a string or a number depending on the query that supplied it.
 * @param {Object} cultureTypeTestOptions - Map of culture type id to allowed isolate test config names.
 * @param {string|number} cultureTypeId - Culture type list item id.
 * @returns {Array<string>} Allowed config names, empty when the culture type has no configuration.
 */
const getAllowedTestsForCultureType = (cultureTypeTestOptions, cultureTypeId) => {
    if (!cultureTypeTestOptions || cultureTypeId == null || cultureTypeId === '') {
        return [];
    }

    const allowedTests = cultureTypeTestOptions[cultureTypeId]
        ?? cultureTypeTestOptions[String(cultureTypeId)]
        ?? cultureTypeTestOptions[Number(cultureTypeId)];

    return Array.isArray(allowedTests) ? allowedTests : [];
}

/**
 * Limits the isolate test selection page to the tests configured against the culture type.
 * The backend already filters the list, so this only hides tests that arrive without being applicable.
 * When the culture type has no configuration, every isolate test stays visible.
 * Matching is on normalized config names, never on translated titles.
 * @param {Array} contents - Crafted pages from the form response.
 * @param {Object} cultureTypeTestOptions - Map of culture type id to allowed isolate test config names.
 * @param {string|number|null|undefined} cultureTypeId - Culture type list item id for the isolate.
 * @param {string} pageName - Name of the crafted page to update.
 * @returns {void}
 */
const SetCultureTestsByCultureType = (contents, cultureTypeTestOptions, cultureTypeId, pageName) => {
    const elementToChange = contents.findIndex((page) => { return page.Name === pageName});

    if (elementToChange < 0) {
        return;
    }

    const allowedTests = getAllowedTestsForCultureType(cultureTypeTestOptions, cultureTypeId);
    const allowedFormNames = new Set(allowedTests.map((testName) => normalizeCultureTestFormName(testName)));

    for (const item of contents[elementToChange].Contents) {
        if (item.Key === "XCategX") {
            continue;
        }

        if (allowedFormNames.size === 0) {
            item.Display = 'Yes';
            continue;
        }

        item.Display = allowedFormNames.has(normalizeCultureTestFormName(item.Key)) ? 'Yes' : 'No';
    }
}

/**
 * Attaches the laboratory test category definitions to a selection page so the category filter can render.
 * @param {Array} contents - Crafted pages from the form response.
 * @param {Array} categories - Category definitions for the laboratory.
 * @param {string} pageName - Name of the crafted page to update.
 * @param {boolean} clearBefore - When true, any existing category entry is removed first.
 * @returns {void}
 */
const SetTestCategories = (contents, categories, pageName, clearBefore) => {
    
    const elementToChange = contents.findIndex((page) => { return page.Name === pageName})

    if (elementToChange > -1) {
        if (clearBefore) {
            contents[elementToChange].Contents = contents[elementToChange].Contents.filter((c) => c.Key !== "XCategX");
        }

        contents[elementToChange].Contents.push({Key: "XCategX" , Categories: categories});
    }
}

export default SetDefaultTestsBySpecimenType;

export {SetTestCategories, SetCultureTestsByCultureType}

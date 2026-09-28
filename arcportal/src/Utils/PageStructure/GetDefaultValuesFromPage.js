
/**
 * Returns ISO date (YYYY-MM-DD) and time (HH:mm) strings for the current moment.
 * @returns {{ date: string, time: string }}
 */
const formatNowDefaults = () => {
    const currentDate = new Date();
    const pad = (n) => String(n).padStart(2, '0');
    return {
        date: `${currentDate.getFullYear()}-${pad(currentDate.getMonth() + 1)}-${pad(currentDate.getDate())}`,
        time: `${pad(currentDate.getHours())}:${pad(currentDate.getMinutes())}`
    };
};

/**
 * Whether the field should default to the current date or time per page config.
 * @param {object} field
 * @returns {boolean}
 */
const fieldDefaultsToNow = (field) => field.DefaultToNow === true || field.defaultToNow === true;

/**
 * Whether the field has a static default value (not a dependent placeholder).
 * @param {object} field
 * @returns {boolean}
 */
const hasStaticDefaultValue = (field) => {
    const defaultValue = field.DefaultValue ?? field.defaultValue;
    if (defaultValue === undefined || defaultValue === '') {
        return false;
    }
    return !(defaultValue.includes('<:') && defaultValue.includes(':>'));
};

/**
 * Collects default field values from page structure for keys not already present in existingValues.
 * Applies static DefaultValue or defaultToNow (date → YYYY-MM-DD, time → HH:mm).
 * Also used when a form repeat clears tail pages (see ApplyDefaultsToClearedPages).
 * @param {object[]} pageStructure - Form pages with Columns / FormGroups / Fields.
 * @param {string[]|null} existingValues - Property names already supplied by initial query or list data.
 * @returns {Record<string, string>}
 */
const GetDefaultValuesFromPage = (pageStructure, existingValues) => {

    let defaultValues = {};
    for (const page of pageStructure) {
        for (const column of page.Columns) {
            for (const formGroup of column.FormGroups) {
                for (const field of formGroup.Fields) {
                    var matchIndex = -1;
                    if (existingValues !== null) {
                        matchIndex = existingValues.findIndex(item => item.toLowerCase() === field.Id.toLowerCase());
                    }
                    if (matchIndex !== -1) {
                        continue;
                    }
                    if (hasStaticDefaultValue(field)) {
                        const defaultValue = field.DefaultValue ?? field.defaultValue;
                        defaultValues[field.Id] = defaultValue;
                    } else if (fieldDefaultsToNow(field)) {
                        const now = formatNowDefaults();
                        switch ((field.Type ?? field.type ?? '').toLowerCase()) {
                            case 'date':
                                defaultValues[field.Id] = now.date;
                                break;
                            case 'time':
                                defaultValues[field.Id] = now.time;
                                break;
                            default:
                                defaultValues[field.Id] = now.time;
                        }
                    }
                }
            }
        }

    }
    
    return defaultValues;

};

/**
 * Applies dependent default placeholders (<:fieldId:>) when a matching field changes.
 * @param {object[]} pageStructure
 * @param {string} id - Changed field id.
 * @param {*} value - New value for the field.
 * @returns {{ id: string, value: * }[]}
 */
const SetDynamicDefaultValues = (pageStructure, id, value) => {

    let defaultValues = [];
    for (const page of pageStructure) {
        for (const column of page.Columns) {
            for (const formGroup of column.FormGroups) {
                for (const field of formGroup.Fields) {
                    const defaultValue = field.DefaultValue ?? field.defaultValue;
                    const currentValue = field.value;
                    const targetIsEmpty = currentValue === undefined || currentValue === null || currentValue === '';
                    if (defaultValue !== undefined && targetIsEmpty) {
                        if (defaultValue.includes('<:') && defaultValue.includes(':>')) {
                            const fieldToMatch = defaultValue.replace('<:',"").replace(":>","").trim().toLowerCase();
                            if (id.toLowerCase() === fieldToMatch) {
                                field.value = value;
                                defaultValues.push({id: field.Id, value: value})
                            }
                        }
                    }
                }
            }
        }

    }
    
    return defaultValues;

};

export default GetDefaultValuesFromPage;
export {SetDynamicDefaultValues};

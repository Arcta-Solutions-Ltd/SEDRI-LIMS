/**
 * Resolves dropdown options supplied by an initial query (pageOptions, formGroupOptions).
 * Filters by parentKey when parentList is set on the field config.
 * @param {Object|null|undefined} data - Initial query result on the form.
 * @param {string} optionsName - Field optionsName (e.g. pageOptions, formGroupOptions).
 * @param {Object} config - Field config (ParentList).
 * @param {string|null|undefined} parentOverride - When set, filters by this parent value instead of data[parentList].
 * @returns {Array<{key: string, text: string, ParentKey?: string}>|null}
 */
const resolveQueryDropdownOptions = (data, optionsName, config, parentOverride) => {
    if (!data || !optionsName) {
        return null;
    }

    const dataKey = optionsName.charAt(0).toUpperCase() + optionsName.slice(1);
    const raw = data[optionsName] ?? data[dataKey];
    if (!Array.isArray(raw)) {
        return null;
    }

    const parentList = config?.ParentList ?? config?.parentList ?? '';
    let filtered = raw;

    if (parentList !== '') {
        const parentValue = parentOverride !== undefined
            ? parentOverride
            : (data[parentList] ?? data[parentList.charAt(0).toUpperCase() + parentList.slice(1)]);
        if (parentValue == null || parentValue === '') {
            filtered = [];
        } else {
            filtered = raw.filter((option) => (option.parentKey ?? option.ParentKey) === parentValue);
        }
    }

    return filtered.map((option) => ({
        key: option.key ?? option.Key,
        text: option.text ?? option.Text ?? '',
        ParentKey: option.parentKey ?? option.ParentKey
    }));
};

export default resolveQueryDropdownOptions;

import { GetPropertyValueIgnoreCase } from '../General/FindCaseInsensitiveProperty';

const HIERARCHY_ADD_BY_OPTIONS_NAME = {
    organisationlist: {
        uiEvent: 'addorganisationuievent',
        addLabelTag: '@OrgAddB@',
        addChildLabelTag: '@OrgAddChild@',
        addChildContext: true,
        prefillFormFields: {
            ParentOrganisationId: 'id',
            ParentOrganisation: 'organisationname',
        },
        listName: 'OrganisationList',
    },
    locationlist: {
        uiEvent: 'addlocationuievent',
        addLabelTag: '@LocAdd@',
        addChildLabelTag: '@LocAddChild@',
        addChildContext: true,
        prefillFormFields: {
            ParentLocationId: 'id',
        },
        listName: 'LocationList',
    },
};

/**
 * Maps flat list API options to hierarchical picker option shape.
 * @param {Object|Array} listData - Single list or array of lists from list/get.
 * @param {string} [listName] - Expected list name when listData is an array.
 * @returns {Array<{key: *, text: string, ParentKey: *}>}
 */
const mapListOptions = (listData, listName) => {
    let list = listData;
    if (Array.isArray(listData)) {
        const expected = (listName || '').toLowerCase();
        list = listData.find(
            (entry) => (entry.Name || entry.name || '').toLowerCase() === expected
        ) || listData[0];
    }
    const options = list?.Options || list?.options || [];
    return options.map((option) => ({
        key: option.Key ?? option.key,
        text: option.Text ?? option.text ?? String(option.Key ?? option.key ?? ''),
        ParentKey: option.ParentKey ?? option.parentKey ?? option.parentkey,
    }));
};

/**
 * Resolves add-form configuration for org/location hierarchical pickers.
 * Uses OptionsName convention (Option B) with optional field-level overrides.
 * Set allowAdd: false to suppress add (filter context defaults to false via FilterHierarchyPicker).
 * Set allowAdd: true on data-entry form fields to enable in-place add when the user has permission.
 * @param {Object} fieldConfig - Hierarchical picker field or filter config.
 * @param {Array<{Name: string}>} [uievents] - Allowed UI events for the current user.
 * @returns {Object|null} Add action config, or null when add is not allowed.
 */
const getHierarchyPickerAddConfig = (fieldConfig, uievents) => {
    if (!fieldConfig) {
        return null;
    }

    if (fieldConfig.AllowAdd === false || fieldConfig.allowAdd === false) {
        return null;
    }

    const optionsName = (fieldConfig.OptionsName || fieldConfig.optionsName || '').toLowerCase();
    const baseConfig = HIERARCHY_ADD_BY_OPTIONS_NAME[optionsName];
    if (!baseConfig) {
        return null;
    }

    const uiEventName = fieldConfig.AddFormUIEvent || fieldConfig.addFormUIEvent || baseConfig.uiEvent;
    const hasPermission = Array.isArray(uievents) && uievents.some(
        (entry) => (entry.Name || entry.name || '').toLowerCase() === uiEventName.toLowerCase()
    );
    if (!hasPermission) {
        return null;
    }

    return {
        ...baseConfig,
        uiEvent: uiEventName,
    };
};

/**
 * Builds localFormData.values for add-child forms from a selected tree node.
 * @param {Object} addConfig - Config from getHierarchyPickerAddConfig.
 * @param {Object} contextNode - Selected tree node in the picker callout.
 * @returns {Object|undefined} Prefill values for FormHandler localFormData.
 */
const buildAddChildPrefillValues = (addConfig, contextNode) => {
    if (!addConfig?.addChildContext || !contextNode || !addConfig.prefillFormFields) {
        return undefined;
    }

    const values = {};
    for (const [formField, recordField] of Object.entries(addConfig.prefillFormFields)) {
        const val = GetPropertyValueIgnoreCase(contextNode, recordField);
        if (val !== undefined && val !== null && val !== '') {
            values[formField] = val;
        }
    }

    return Object.keys(values).length > 0 ? values : undefined;
};

export {
    getHierarchyPickerAddConfig,
    mapListOptions,
    buildAddChildPrefillValues,
};

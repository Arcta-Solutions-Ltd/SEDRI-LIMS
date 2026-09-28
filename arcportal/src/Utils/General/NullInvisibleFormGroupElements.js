import EvaluateRules from '../Rules/EvaluateRules';

/** Field ids that must stay on form data even when their form group is not visible. */
const isWorkflowContextFieldId = (fieldId) => {
    if (fieldId === undefined || fieldId === null) return false;
    const id = String(fieldId).toLowerCase();
    return id === 'specimentypeid' || id === 'laboratoryid';
};

/**
 * Sets a form data property to null using case-insensitive key matching.
 * @param {object} data - Form data object to mutate.
 * @param {string} fieldId - Field id to null.
 */
const nullFieldValue = (data, fieldId) => {
    const property = Object.keys(data).find(key => key.toLowerCase() === String(fieldId).toLowerCase());
    if (property !== undefined) {
        data[property] = null;
    } else {
        data[fieldId] = null;
    }
};

/**
 * Clears field values belonging to form groups that are not visible according to their visibility rules.
 * Evaluates rules against the merged save payload so hidden mutual-exclusive fields are not posted.
 * @param {{ Pages: object[], data: object }} formDef - Form definition with pages and merged data.
 * @returns {object} Updated form data with hidden form-group field values set to null.
 */
const NullInvisibleFormGroupElements = (formDef, currentPageName) => {
    const newData = { ...formDef.data };

    if (!Array.isArray(formDef.Pages)) {
        return newData;
    }

    for (const page of formDef.Pages) {
        const isCurrentPage = currentPageName &&
            String(page.Name ?? page.name ?? '').toLowerCase() === String(currentPageName).toLowerCase();

        if (!Array.isArray(page.Columns)) {
            continue;
        }

        for (const column of page.Columns) {
            if (!Array.isArray(column.FormGroups)) {
                continue;
            }

            for (const formGroup of column.FormGroups) {
                if (isCurrentPage) {
                    continue;
                }

                const rules = formGroup.Rules ?? formGroup.rules;
                if (!rules || rules.length === 0) {
                    continue;
                }

                const visible = EvaluateRules('visible', rules, newData);
                if (visible) {
                    continue;
                }

                const fields = formGroup.Fields ?? formGroup.fields ?? [];
                for (const field of fields) {
                    const fieldId = field.Id ?? field.id;
                    if (fieldId === undefined || fieldId === null || isWorkflowContextFieldId(fieldId)) {
                        continue;
                    }
                    nullFieldValue(newData, fieldId);
                }
            }
        }
    }

    return newData;
};

export default NullInvisibleFormGroupElements;

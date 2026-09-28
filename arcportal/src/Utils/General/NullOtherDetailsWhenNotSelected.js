import { isOtherDetailsFieldVisible } from '../Forms/OtherOptionConstants';

/**
 * Sets companion "Other details" field values to null when the parent list field is not "Other".
 * @param {{ Pages: object[], data: object }} formDef
 * @returns {object} Updated form data.
 */
const NullOtherDetailsWhenNotSelected = (formDef) => {
    const newData = { ...formDef.data };

    if (!Array.isArray(formDef.Pages)) {
        return newData;
    }

    for (const page of formDef.Pages) {
        for (const column of page.Columns ?? []) {
            for (const formGroup of column.FormGroups ?? []) {
                for (const field of formGroup.Fields ?? []) {
                    const otherDetailsFor = field.OtherDetailsFor ?? field.otherDetailsFor;
                    if (!otherDetailsFor) {
                        continue;
                    }

                    if (isOtherDetailsFieldVisible(field, newData, formGroup.Fields)) {
                        continue;
                    }

                    const fieldId = field.Id ?? field.id;
                    const property = Object.keys(newData).find(
                        key => key.toLowerCase() === String(fieldId).toLowerCase()
                    );
                    if (property !== undefined) {
                        newData[property] = null;
                    } else {
                        newData[fieldId] = null;
                    }
                }
            }
        }
    }

    return newData;
};

export default NullOtherDetailsWhenNotSelected;

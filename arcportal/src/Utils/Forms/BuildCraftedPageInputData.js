import ExtractFieldListFromPageStructure from '../PageStructure/ExtractFieldListFromPageStructure';
import ExtractAgeRangeParametersFromFormData from '../PageStructure/ExtractAgeRangeParametersFromFormData';
import NormalizeCraftedPageContents from '../Crafted/NormalizeCraftedPageContents';

/**
 * Gathers everything a crafted page can be driven by: the values held on every page of the form, the contents
 * of every crafted page, the ArcAge range values that live on the form data rather than on a field, and the
 * parent record identifiers chosen by earlier selection pages. Used both when a crafted page is navigated to
 * and when the form opens straight onto one.
 *
 * @param {object} formDef - The form definition, with its Pages and data.
 * @returns {Array<{key: string, value: *}>} Key/value entries for the crafted page.
 */
const BuildCraftedPageInputData = (formDef) => {
    const inputData = ExtractFieldListFromPageStructure(formDef.Pages);

    for (const crafted of formDef.data?.Crafted ?? []) {
        for (const item of NormalizeCraftedPageContents(crafted.Contents)) {
            inputData.push(item);
        }
    }

    for (const ageParam of ExtractAgeRangeParametersFromFormData(formDef.data)) {
        const existingIndex = inputData.findIndex(
            (item) => (item.key ?? item.Key)?.toLowerCase() === ageParam.key.toLowerCase()
        );
        if (existingIndex === -1) {
            inputData.push(ageParam);
        } else {
            inputData[existingIndex] = ageParam;
        }
    }

    for (const identifier of ['PatientId', 'AdmissionId', 'RequestId']) {
        const value = formDef.data?.[identifier];
        if (value !== undefined && value !== null &&
            !inputData.some((item) => (item.key ?? item.Key)?.toLowerCase() === identifier.toLowerCase())) {
            inputData.push({ key: identifier, value: value });
        }
    }

    return inputData;
};

export default BuildCraftedPageInputData;

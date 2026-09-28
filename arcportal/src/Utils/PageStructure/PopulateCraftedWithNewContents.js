import Undefined from '../General/Undefined';
import AddValuesIntoArrayList from '../Forms/AddValuesIntoArrayList';
import SetDefaultTestsBySpecimenType, {SetTestCategories, SetCultureTestsByCultureType} from './SetDefaultTestsBySpecimenType';
import LaboratoryList from '../../Classes/Laboratory/LaboratoryList';
import TokenInfo from '../../Classes/Security/TokenInfo';

/**
 * Merges query results into a crafted page and reapplies the laboratory test defaults when the
 * specimen type or laboratory changes.
 * @param {Object} formDef - Form definition being populated, mutated in place.
 * @param {string} pageName - Name of the crafted page receiving the new contents.
 * @param {Array} data - Values returned from the query.
 * @param {string} id - Id of the field that triggered the repopulation.
 * @param {Array} laboratoryConfig - Laboratory configuration from Redux.
 * @returns {void}
 */
const PopulateCraftedWithNewContents = (formDef, pageName, data, id, laboratoryConfig) => {

    if (! Undefined(formDef.data.Crafted)) {
        for (const crafted of formDef.data.Crafted) {
            if (crafted.Name.toLowerCase() === pageName.toLowerCase()) {
                crafted.Contents = AddValuesIntoArrayList(crafted.Contents, data);
                formDef.craftedPageData = crafted.Contents;
            }
        }

        /*Handle setting up default tests and culture types defined by specimen types*/ 

        if (id === "specimentypeid" || id === "laboratoryid") {
            if (formDef.data.LaboratoryId === undefined) {
                var token = new TokenInfo("arctoken");
                formDef.data.LaboratoryId = token.tokenInfo.LaboratoryId;
            }
            SetTestAndCultureTypeDisplays(formDef.data.Crafted, formDef.data.SpecimenTypeId, laboratoryConfig, formDef.data.LaboratoryId, false);
        }
    }
}

/**
 * Applies the laboratory configuration to the test and culture type selection pages of a form.
 * The isolate test page is limited by the culture type isolate test options; the culture type page
 * is limited by the specimen type culture type options.
 * @param {Array} data - Crafted pages from the form response.
 * @param {string|number} specimenTypeId - Specimen type list item id.
 * @param {Array} laboratoryConfig - Laboratory configuration from Redux.
 * @param {string|number} laboratoryId - Laboratory the specimen belongs to.
 * @param {boolean} ignoreDefaults - When true, existing Allowed values are preserved.
 * @param {string|number|null|undefined} cultureTypeId - Culture type list item id when selecting isolate tests.
 * @returns {void}
 */
const SetTestAndCultureTypeDisplays = (data, specimenTypeId, laboratoryConfig, laboratoryId, ignoreDefaults, cultureTypeId) => {

    const laboratoryListToUse = new LaboratoryList(laboratoryConfig, specimenTypeId);
    const laboratory = laboratoryListToUse.getLaboratory(laboratoryId);

    SetDefaultTestsBySpecimenType(data, laboratory.DirectTestOptions, laboratory.DirectTestDefaults, "testselectionpage", ignoreDefaults);

    // Isolate tests are limited by culture type only. Passing an unknown culture type leaves every test visible,
    // which matches the backend behaviour when no culture type isolate test options are configured.
    SetCultureTestsByCultureType(data, laboratory.CultureTypeTestOptions, cultureTypeId == null ? cultureTypeId : cultureTypeId.toString(), "testcultureselectionpage");

    SetDefaultTestsBySpecimenType(data, laboratory.CultureTypeOptions, laboratory.CultureTypeDefaults, "culturetypeselectionpage", ignoreDefaults);

    SetTestCategories(data,laboratory.TestCategory,"testselectionpage", true);
    SetTestCategories(data,laboratory.CultureTypeCategory,"culturetypeselectionpage", false);
}


export default PopulateCraftedWithNewContents;

export { SetTestAndCultureTypeDisplays }




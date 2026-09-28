import csvToArray from "../../Utils/General/CSVToArray";

class Laboratory {

    constructor(laboratory, specimenTypeId) {
        this.LaboratoryId = laboratory.LaboratoryId;
        this.WorkflowId = Number(laboratory.DefaultWorkflowId);
        this.ApproveReports = laboratory.ApproveReports;
        this.RecordSusceptibilityChangeAudit = laboratory.RecordSusceptibilityChangeAudit;

        // Keyed on form name rather than specimen type, so it is built whether or not a specimen type is known.
        this.FormSpecimenTypeOptions = this.#formatFormSpecimenTypeOptions(laboratory.Configuration);

        if (specimenTypeId != null && specimenTypeId !== "") {
            this.#convertLaboratoryToDefaultTestStructure(laboratory, specimenTypeId);
        }
    }

    /**
     * Formats the per-form specimen type restrictions into a map of form name to allowed specimen type ids.
     * A form with no entry is unrestricted.
     *
     * @param {array} info - The laboratory configuration list.
     * @returns {object} - Map of lower case form name to allowed specimen type id arrays.
     */
    #formatFormSpecimenTypeOptions = (info) => {
        const optionsMap = {};
        const matchingItems = (info || []).filter(item => item != null && item.ConfigName === "formspecimentypeoption");
        for (const item of matchingItems) {
            if (item.GroupId != null && item.GroupId !== "") {
                optionsMap[item.GroupId.toString().toLowerCase()] = csvToArray(item.AssociatedListId);
            }
        }
        return optionsMap;
    }

    /**
     * Converts laboratory configuration data into a structured test format.
     *
     * @param {number} labId - The laboratory ID.
     * @param {object} laboratoryInfo - The laboratory configuration data.
     * @param {number} group - The group ID used for filtering.
     * @returns {object} - Structured test data for the given laboratory and group.
     */
    #convertLaboratoryToDefaultTestStructure = (laboratory, specimenTypeId) => {

        const configuration = (laboratory.Configuration || []).filter(
            (c) => c != null && c.GroupId != null && c.GroupId.toString() === specimenTypeId.toString()
        );

        this.TestCategory = this.#formatCategoryData("testcategory", laboratory.Configuration);
        this.CultureTypeCategory = this.#formatCategoryData("culturetypecategory", laboratory.Configuration);
        this.DirectTestOptions = this.#formatData("specimentypedirecttestoption", configuration);
        this.DirectTestDefaults = this.#formatData("specimentypedirecttestdefault", configuration);
        this.CultureTypeOptions = this.#formatData("specimentypeculturetypeoption", configuration);
        this.CultureTypeDefaults = this.#formatData("specimentypeculturetypedefault", configuration);
        this.CultureTypeTestOptions = this.#formatCultureTypeTestOptions(laboratory.Configuration);
        this.WorkflowId = this.#GetWorkflowId(laboratory.Configuration, specimenTypeId);
    }

    #GetWorkflowId = (config, specimenTypeId) => {
        let returnId = this.WorkflowId
        if (specimenTypeId !== undefined && config) {
            const workflowConfigItems = config.filter(item => item != null && item.ConfigName === "specimentypeworkflow");

            for (const configItem of workflowConfigItems) {
                if (configItem?.AssociatedListId && configItem.AssociatedListId.split(",").includes(specimenTypeId.toString())) {
                    returnId = Number(configItem.GroupId);
                }
            }
        }
        return returnId;
    }

    /**
     * Formats specific test data based on the configuration type.
     *
     * @param {string} type - The type of test data to format.
     * @param {array} info - The laboratory configuration list.
     * @returns {object|undefined} - Formatted test data or undefined if no match is found.
     */
    #formatData = (type, info) => {

        const matchingItem = info?.find(item => item != null && item.ConfigName === type);
        return matchingItem === undefined || matchingItem?.GroupId == null ? undefined : { Group: Number(matchingItem.GroupId), Values: csvToArray(matchingItem.AssociatedListId)};
    }

    /**
     * Formats category data for test configurations.
     *
     * @param {string} type - The category type.
     * @param {array} info - The laboratory configuration list.
     * @returns {array} - List of categorized test data.
     */
    #formatCategoryData = (type, info) => {

        var categoryList = [];
        const matchingItems = (info || []).filter(item => item != null && item.ConfigName === type);
        for (const item of matchingItems) {
            if (item.GroupId != null) {
                categoryList.push({ CategoryId: item.GroupId, Category: item.GroupText, Values: csvToArray(item.AssociatedListId) });
            }
        }
        return categoryList;
    }

    /**
     * Formats culture type test options into a map structure.
     * Maps culture type IDs to arrays of test names. Where several configuration rows exist
     * for the same culture type their test names are merged, matching the server-side resolver.
     *
     * @param {array} info - The laboratory configuration list.
     * @returns {object} - Map of culture type IDs to test name arrays.
     */
    #formatCultureTypeTestOptions = (info) => {
        const matchingItems = (info || []).filter(item => item != null && item.ConfigName === "culturetypeculturetestoption");
        const optionsMap = {};
        for (const item of matchingItems) {
            if (item.GroupId != null) {
                const cultureTypeId = item.GroupId;
                const testNames = csvToArray(item.AssociatedListId);
                const existingNames = optionsMap[cultureTypeId] || [];
                optionsMap[cultureTypeId] = [...existingNames, ...testNames.filter(name => !existingNames.includes(name))];
            }
        }
        return optionsMap;
    }
}

export default Laboratory

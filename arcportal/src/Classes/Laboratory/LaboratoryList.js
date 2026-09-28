import Laboratory from "./Laboratory";

/**
 * Wraps laboratory configuration for lookup by laboratory ID.
 * Provides CultureTypeTestOptions and other lab-specific settings per specimen type.
 */
class LaboratoryList {

    /**
     * Creates a LaboratoryList from laboratory configuration.
     * @param {Object} laboratoryConfig - Config object with LaboratoryList array
     * @param {number} specimenTypeId - Specimen type ID for filtering lab settings
     */
    constructor(laboratoryConfig, specimenTypeId) {
        const list = laboratoryConfig?.LaboratoryList;
        this.laboratoryArray = Array.isArray(list)
            ? list.map(lab => new Laboratory(lab, specimenTypeId))
            : [];
    }

    /**
     * Returns the laboratory config for the given laboratory ID.
     * @param {number|string} laboratoryid - Laboratory ID to look up
     * @returns {Laboratory|undefined} Laboratory instance or undefined
     */
    getLaboratory(laboratoryid) {
        return laboratoryid !== undefined
            ? this.laboratoryArray.find(l => l.LaboratoryId === Number(laboratoryid))
            : undefined;
    }
}

export default LaboratoryList





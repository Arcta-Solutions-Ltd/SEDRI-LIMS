import AddJsonValuesIntoPageStructure from '../PageStructure/AddJsonValuesIntoPageStructure';
import { formatAgeFromDateOfBirthWithTags } from '../General/FormatAgeDisplay';

const PATIENT_PAGES_WITH_DOB = [
    'patientdetailspage',
    'editpatientdetailspage',
    'neoshieldbirthdetailspage',
];

/**
 * Returns true when the rule should run for the given context.
 *
 * @param {Object} formDef - Form definition.
 * @param {Object} context - Rule context.
 * @returns {boolean}
 */
const ruleApplies = (formDef, context) => {
    if (context.trigger === 'fieldChange') {
        const fieldId = (context.changedFieldId ?? '').toLowerCase();
        return fieldId === 'dateofbirth';
    }

    if (context.trigger === 'initialLoad') {
        return formDef.Pages?.some((p) => PATIENT_PAGES_WITH_DOB.includes((p.Name ?? p.name ?? '').toLowerCase()));
    }

    if (context.trigger === 'pageNavigation') {
        const pageName = (context.newPage?.Name ?? context.newPage?.name ?? '').toLowerCase();
        return PATIENT_PAGES_WITH_DOB.includes(pageName);
    }

    return false;
};

/**
 * Form data rule that calculates and displays patient age from DateOfBirth.
 * Writes to PatientAgeDisplay only; does not persist age on the patient record.
 *
 * @param {Object} formDef - The form definition with data and Pages.
 * @param {Object} context - { trigger, changedFieldId?, newPage? }
 * @returns {Object} The (possibly modified) formDef.
 */
const UpdatePatientAgeDisplayRule = (formDef, context) => {
    if (!ruleApplies(formDef, context)) {
        return formDef;
    }

    const data = formDef.data || {};
    const dob = data.DateOfBirth ?? data.dateofbirth;
    const dobVal = typeof dob === 'object' && dob?.value !== undefined ? dob.value : dob;

    const displayValue = dobVal ? formatAgeFromDateOfBirthWithTags(dobVal || dob) : '';

    formDef.data = {
        ...formDef.data,
        PatientAgeDisplay: displayValue,
    };
    formDef.Pages = AddJsonValuesIntoPageStructure(formDef.Pages, formDef.data, true);

    return formDef;
};

export default UpdatePatientAgeDisplayRule;

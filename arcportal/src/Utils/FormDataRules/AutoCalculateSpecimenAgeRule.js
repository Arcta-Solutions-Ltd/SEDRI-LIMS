import CalculateAgeFromDateOfBirth from '../General/CalculateAgeFromDateOfBirth';
import AddJsonValuesIntoPageStructure from '../PageStructure/AddJsonValuesIntoPageStructure';

/**
 * Form data rule that auto-calculates specimen age (years, months, days, hours) from DateOfBirth
 * when the form has specimen timing pages and age fields are empty.
 *
 * Applies when:
 * - context.trigger === 'initialLoad' and formDef.Pages has a page named 'specimentimings', OR
 * - context.trigger === 'pageNavigation' and context.newPage?.Name is 'specimentimings' or 'advancespecimendetailspage'.
 *
 * Condition: data.DateOfBirth (or dateofbirth) exists; AgeYears, AgeMonths, AgeDays, AgeHours are all empty.
 *
 * Action: Calculates age from DOB to reference date (CollectionDate/ReceivedDate or now), merges into formDef.data,
 * and updates formDef.Pages via AddJsonValuesIntoPageStructure.
 *
 * @param {Object} formDef - The form definition with data and Pages.
 * @param {Object} context - { trigger: 'initialLoad' | 'pageNavigation', newPage?: { Name: string } }
 * @returns {Object} The (possibly modified) formDef.
 */
const AutoCalculateSpecimenAgeRule = (formDef, context) => {
    const appliesOnInitialLoad =
        context.trigger === 'initialLoad' &&
        formDef.Pages?.some((p) => p.Name === 'specimentimings');

    const appliesOnPageNav =
        context.trigger === 'pageNavigation' &&
        (context.newPage?.Name === 'specimentimings' || context.newPage?.Name === 'advancespecimendetailspage');

    if (!appliesOnInitialLoad && !appliesOnPageNav) {
        return formDef;
    }

    const data = formDef.data || {};
    const dob = data.DateOfBirth ?? data.dateofbirth;
    const dobVal = typeof dob === 'object' && dob?.value !== undefined ? dob.value : dob;
    const ageEmpty =
        (data.AgeYears ?? data.ageyears ?? '') === '' &&
        (data.AgeMonths ?? data.agemonths ?? '') === '' &&
        (data.AgeDays ?? data.agedays ?? '') === '' &&
        (data.AgeHours ?? data.agehours ?? '') === '';

    if (!dob || !ageEmpty) {
        return formDef;
    }

    const refDate = data.CollectionDate ?? data.collectiondate ?? data.ReceivedDate ?? data.receiveddate;
    const age = CalculateAgeFromDateOfBirth(dobVal || dob, refDate);

    formDef.data = {
        ...formDef.data,
        AgeYears: age.Years,
        AgeMonths: age.Months,
        AgeDays: age.Days,
        AgeHours: age.Hours,
    };
    formDef.Pages = AddJsonValuesIntoPageStructure(formDef.Pages, formDef.data, true);

    return formDef;
};

export default AutoCalculateSpecimenAgeRule;

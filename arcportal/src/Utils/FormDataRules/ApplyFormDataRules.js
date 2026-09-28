import AutoCalculateSpecimenAgeRule from './AutoCalculateSpecimenAgeRule';
import UpdatePatientAgeDisplayRule from './UpdatePatientAgeDisplayRule';

/**
 * Registered form data rules. Each rule is a function (formDef, context) => formDef.
 * Rules are applied in order; each receives the output of the previous.
 *
 * To add a new rule: create a rule function and push it to the rules array.
 */
const rules = [AutoCalculateSpecimenAgeRule, UpdatePatientAgeDisplayRule];

/**
 * Applies all registered form data rules to the form definition.
 *
 * @param {Object} formDef - The form definition with data and Pages.
 * @param {Object} context - Context for rule application.
 *   - trigger: 'initialLoad' | 'pageNavigation' | 'fieldChange'
 *   - newPage?: { Name: string } - The page being navigated to (when trigger is 'pageNavigation').
 *   - changedFieldId?: string - The field id that changed (when trigger is 'fieldChange').
 * @returns {Object} The formDef after all rules have been applied.
 */
const ApplyFormDataRules = (formDef, context) => {
    let result = formDef;
    for (const rule of rules) {
        result = rule(result, context);
    }
    return result;
};

export default ApplyFormDataRules;

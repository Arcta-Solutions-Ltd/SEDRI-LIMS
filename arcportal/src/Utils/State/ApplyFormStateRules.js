import EvaluateRules from "../Rules/EvaluateRules";

const ApplyFormStateRules = (formDef, state) => {

    state = state === undefined ? "" : state;

    for (const page of formDef.Pages) {
        page.Visible = true;
        for (const rule of formDef.Rules) {
            if (page.Name === rule.Page) {
                if ( (! state.includes(rule.State)) && rule.Outcome.toLowerCase() === "visible") {
                    page.Visible = false
                }
            }
        }
    }
}

/**
 * Collects distinct effect tokens from onClickState rules, preserving first-seen order.
 * @param {Array<{Effect?: string, effect?: string}>} rules - OnClickState rules.
 * @returns {string[]} Distinct effect tokens.
 */
const getDistinctEffects = (rules) => {
    const effects = [];
    if (!rules || rules.length === 0) {
        return effects;
    }

    for (const rule of rules) {
        const effect = rule.Effect ?? rule.effect ?? "";
        if (effect && !effects.includes(effect)) {
            effects.push(effect);
        }
    }

    return effects;
}

/**
 * Adds or removes a state token from a comma-separated state string.
 * @param {string} currentState - Current comma-separated state.
 * @param {string} token - Token to add or remove.
 * @param {boolean} shouldInclude - True to add, false to remove.
 * @returns {string} Updated state string.
 */
const setStateToken = (currentState, token, shouldInclude) => {
    let newState = currentState ?? "";

    if (shouldInclude) {
        if (!newState.includes(token)) {
            newState = newState === "" ? token : newState + "," + token;
        }
        return newState;
    }

    if (newState === "") {
        return newState;
    }

    const stateArray = newState.split(",").filter((item) => item !== token);
    return stateArray.toString();
}

/**
 * Applies onClickState rules when the user moves on. Each distinct effect in the rules list is
 * evaluated independently; rows sharing the same effect are ANDed. When no rules exist but State is
 * set, the primary state is added or removed unconditionally (legacy behaviour).
 * @param {Object} page - Current page definition.
 * @param {Object} state - Form state including currentState and formDef.data.
 * @param {string} sourceOfClick - Source of the navigation click.
 * @returns {string} Updated comma-separated state.
 */
const CalculateStateFromFormStateRules = (page, state, sourceOfClick) => {

    let newState = state.currentState;
    const onClickState = page.NextButton?.OnClickState;

    if (onClickState === undefined || onClickState.State === "") {
        return newState;
    }

    state.formDef.data["SourceOfClick"] = sourceOfClick;
    const rules = onClickState.Rules;
    const distinctEffects = getDistinctEffects(rules);

    if (distinctEffects.length === 0) {
        newState = setStateToken(newState, onClickState.State, true);
    } else {
        for (const effect of distinctEffects) {
            const rulesPass = EvaluateRules(effect, rules, state.formDef.data);
            newState = setStateToken(newState, effect, rulesPass);
        }
    }

    delete state.formDef.data.SourceOfClick;

    return newState;
}

/**
 * Returns whether the default next-button layout should be kept. When false, WorkflowEntry
 * recalculates Next vs Finish from projected form state (WillThisBeTheLastPage).
 * Single-effect pages return true when that effect matches (default nav). Branching pages with
 * multiple distinct effects always return false so button visibility tracks field changes.
 * @param {Object} nextButton - Next button config.
 * @param {Object} data - Current form field data.
 * @returns {boolean} True when the default next-button layout should be kept.
 */
const EvaluateFormStateRules = (nextButton, data) => {
    if (nextButton === undefined || nextButton.OnClickState === undefined) {
        return true;
    }

    const onClickState = nextButton.OnClickState;
    const rules = (onClickState.Rules ?? []).filter((f) => (f.Field ?? f.field ?? "").toLowerCase() !== "sourceofclick");

    if (rules.length === 0) {
        return true;
    }

    const distinctEffects = getDistinctEffects(rules);
    if (distinctEffects.length === 0) {
        return EvaluateRules(onClickState.State, rules, data);
    }

    if (distinctEffects.length > 1) {
        return false;
    }

    return EvaluateRules(distinctEffects[0], rules, data);
}

/**
 * Returns whether navigation from the current page should show Finish rather than Next,
 * based on form visibility rules and the comma-separated workflow state tokens.
 * @param {string} state - Comma-separated workflow state tokens.
 * @param {Array<{State: string, Page: string}>} rules - Form page visibility rules.
 * @param {Array<{Name: string}>} pages - Visible pages in display order.
 * @param {string} currentPageName - Name of the current page.
 * @returns {boolean} True when Finish should replace Next on the current page.
 */
const WillThisBeTheLastPage = (state, rules, pages, currentPageName) => {

    const stateTokens = (state ?? "").toLowerCase().split(",").filter((token) => token !== "");
    const matchingRules = rules === undefined ? [] : rules.filter((r) => stateTokens.includes(r.State.toLowerCase()));
    let finalPage = matchingRules.length === 0;

    let allPagesInRules = true
    let currentPageFound = false;
    if (finalPage) {
        for (const page of pages) {
            if (currentPageFound) {
                let matchingPage = rules.filter(r => r.Page.toLowerCase() === page.Name.toLowerCase());
                if (matchingPage.length === 0) {
                    allPagesInRules = false;
                }
            } else {
                currentPageFound = currentPageName == page.Name;
            }
        }
        finalPage = allPagesInRules;
    }

    return finalPage;
}

export default ApplyFormStateRules;

export { CalculateStateFromFormStateRules, EvaluateFormStateRules, WillThisBeTheLastPage, getDistinctEffects }

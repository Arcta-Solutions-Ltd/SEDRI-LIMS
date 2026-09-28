/**
 * Fixed palette of 15 primary colours used to colour-code expert rules on the AST screen.
 * Saturated tones are chosen so the white expert-rule icon glyph stays legible on each colour.
 */
export const EXPERT_RULE_COLOUR_PALETTE = [
    '#d32f2f', // red
    '#1976d2', // blue
    '#388e3c', // green
    '#f57c00', // orange
    '#7b1fa2', // purple
    '#0097a7', // cyan
    '#c2185b', // pink
    '#5d4037', // brown
    '#455a64', // blue grey
    '#00796b', // teal
    '#303f9f', // indigo
    '#e64a19', // deep orange
    '#512da8', // deep purple
    '#0288d1', // light blue
    '#689f38'  // light green
];

/** Neutral fallback colour used when a rule id has no assigned palette colour. */
export const DEFAULT_EXPERT_RULE_COLOUR = '#708090';

/**
 * Builds a deterministic { ruleId: colour } map by assigning palette colours in ascending rule-id order.
 * The mapping is stable across refreshes (keyed by id, not order of arrival); colours cycle when more than
 * 15 distinct rules are present.
 *
 * @param {Array<number|string>} ruleIds - Expert rule ids (from groups and comment alerts).
 * @returns {Object<string,string>} Map keyed by string rule id to a hex colour.
 */
export function buildExpertRuleColourMap(ruleIds) {
    const map = {};
    if (!Array.isArray(ruleIds)) {
        return map;
    }
    const unique = [...new Set(ruleIds.map((id) => Number(id)).filter((id) => Number.isFinite(id)))].sort(
        (a, b) => a - b
    );
    unique.forEach((id, index) => {
        map[String(id)] = EXPERT_RULE_COLOUR_PALETTE[index % EXPERT_RULE_COLOUR_PALETTE.length];
    });
    return map;
}

/**
 * Returns the colour assigned to a rule id, or a neutral default when unknown.
 *
 * @param {Object<string,string>} colourMap - Map from {@link buildExpertRuleColourMap}.
 * @param {number|string} ruleId - Expert rule id.
 * @returns {string} Hex colour.
 */
export function getExpertRuleColour(colourMap, ruleId) {
    if (!colourMap || ruleId === undefined || ruleId === null) {
        return DEFAULT_EXPERT_RULE_COLOUR;
    }
    return colourMap[String(ruleId)] ?? DEFAULT_EXPERT_RULE_COLOUR;
}

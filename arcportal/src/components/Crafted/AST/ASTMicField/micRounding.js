import {
    MICValueMappingsForGuidelines,
    GUIDELINES_CLSI,
    GUIDELINES_EUCAST
} from './MICValueMappings';

/** Largest geometric step in the mapping table (1024). */
const TABLE_MAX_GEOMETRIC = MICValueMappingsForGuidelines[0].Geometric;

/** Geometric dilution steps in ascending order for ceiling lookup. */
const geometricAscending = [...MICValueMappingsForGuidelines]
    .map((row) => row.Geometric)
    .sort((a, b) => a - b);

const FLOAT_COMPARE_EPSILON = 1e-9;

/** Max ratio of (num - G) / (nextStep - num) to treat input as a decimal artefact of G. */
const DECIMAL_ARTIFACT_MAX_RATIO = 0.05;

/** Maximum decimal places considered for finite-precision artefact snap (typical lab entry). */
const DECIMAL_ARTIFACT_MAX_DP = 3;

/**
 * Counts fractional decimal digits in a numeric string.
 *
 * @param {string|null|undefined} numericString - Numeric portion of MIC input (without operator).
 * @returns {number|null} Decimal places when a fractional part is present; null for integers.
 */
export function countDecimalPlaces(numericString) {
    if (!numericString || typeof numericString !== 'string') {
        return null;
    }
    const dotIndex = numericString.indexOf('.');
    if (dotIndex === -1) {
        return null;
    }
    return numericString.length - dotIndex - 1;
}

/**
 * Half-up round to a fixed number of decimal places (typical lab rounding).
 *
 * @param {number} value - Value to round.
 * @param {number} dp - Decimal places.
 * @returns {number}
 */
export function roundHalfUp(value, dp) {
    const factor = Math.pow(10, dp);
    return Math.round(value * factor + Number.EPSILON) / factor;
}

/**
 * Returns the next geometric dilution step above {@code geometricValue}.
 *
 * @param {number} geometricValue - Current geometric step.
 * @returns {number|null} Next step, or null when at table maximum.
 */
function nextGeometricStep(geometricValue) {
    const index = geometricAscending.indexOf(geometricValue);
    if (index === -1 || index >= geometricAscending.length - 1) {
        return null;
    }
    return geometricAscending[index + 1];
}

/**
 * Whether {@code num} is much closer to {@code geometric} than to the next dilution step.
 * Prevents false snaps for mid-range values (e.g. 0.3 is not a 1 dp artefact of 0.25).
 *
 * @param {number} num - Parsed numeric MIC.
 * @param {number} geometric - Candidate geometric step.
 * @returns {boolean}
 */
function isDecimalArtifactProximity(num, geometric) {
    const next = nextGeometricStep(geometric);
    if (next == null) {
        return false;
    }
    const distanceToStep = num - geometric;
    const distanceToNext = next - num;
    if (distanceToStep <= FLOAT_COMPARE_EPSILON || distanceToNext <= FLOAT_COMPARE_EPSILON) {
        return false;
    }
    return distanceToStep / distanceToNext < DECIMAL_ARTIFACT_MAX_RATIO;
}

/**
 * When {@code num} exceeds geometric step {@code G} only because of finite decimal representation
 * (e.g. 0.063 is 0.0625 at 3 dp), return {@code G}; otherwise null.
 *
 * @param {number} num - Parsed numeric MIC.
 * @param {number|null} decimalPlaces - From input string; when null checks dp 1..4.
 * @returns {number|null} Matching geometric step or null.
 */
export function findGeometricStepForDecimalArtifact(num, decimalPlaces) {
    const dpValues = decimalPlaces != null && decimalPlaces > 0
        ? [Math.min(decimalPlaces, DECIMAL_ARTIFACT_MAX_DP)]
        : [1, 2, DECIMAL_ARTIFACT_MAX_DP];

    for (const row of MICValueMappingsForGuidelines) {
        const geometric = row.Geometric;
        if (num <= geometric + FLOAT_COMPARE_EPSILON) {
            continue;
        }
        for (const dp of dpValues) {
            const rounded = roundHalfUp(geometric, dp);
            if (Math.abs(num - rounded) < FLOAT_COMPARE_EPSILON
                && isDecimalArtifactProximity(num, geometric)) {
                return geometric;
            }
        }
    }
    return null;
}

/**
 * Smallest geometric MIC step that is greater than or equal to the input.
 *
 * @param {number} num - Parsed numeric MIC (µg/mL).
 * @returns {number} Canonical geometric step from MICValueMappingsForGuidelines.
 */
function geometricCeiling(num) {
    for (const g of geometricAscending) {
        if (g >= num) {
            return g;
        }
    }
    return geometricAscending[geometricAscending.length - 1];
}

/**
 * Whether geometric-ceiling MIC rounding applies for the active guideline.
 * CLSI, EUCAST, and missing/unknown (0) round; other specific guideline ids pass through unchanged.
 *
 * @param {number|string} guidelinesId - Guidelines list item id.
 * @returns {boolean}
 */
export function shouldApplyMicRounding(guidelinesId) {
    const gid = Number(guidelinesId);
    if (gid === GUIDELINES_CLSI || gid === GUIDELINES_EUCAST) {
        return true;
    }
    return gid === 0;
}

/**
 * Guideline display column for a geometric step.
 *
 * @param {number} geometricValue - Row Geometric value.
 * @param {number} guidelinesId - GUIDELINES_CLSI (971), GUIDELINES_EUCAST (972), or 0 for Geometric.
 * @returns {string}
 */
function guidelineColumnKey(guidelinesId) {
    const gid = Number(guidelinesId);
    if (gid === GUIDELINES_CLSI) {
        return 'CLSI';
    }
    if (gid === GUIDELINES_EUCAST) {
        return 'EUCAST';
    }
    return 'Geometric';
}

/**
 * Returns the EUCAST, CLSI, or Geometric display value for a canonical geometric step.
 *
 * @param {number} geometricValue - Row Geometric value.
 * @param {number|string} guidelinesId - Guidelines list item id.
 * @returns {string|number}
 */
export function getGuidelineMicDisplayForGeometricStep(geometricValue, guidelinesId) {
    const row = MICValueMappingsForGuidelines.find((r) => r.Geometric === geometricValue);
    if (!row) {
        return String(geometricValue);
    }
    return row[guidelineColumnKey(guidelinesId)];
}

/**
 * Rounds a numeric MIC up to the next geometric dilution, then to the EUCAST or CLSI
 * display value for the active guideline. Missing guideline (0) uses the Geometric column.
 * Non-CLSI/EUCAST guideline ids return the input unchanged. If the input already equals a
 * valid display step for that guideline, it is returned unchanged. When the input exceeds a
 * geometric step only because of limited decimal places (e.g. 0.063 for 0.0625 at 3 dp), snaps
 * to that step instead of ceiling-rounding. Rounding down is not permitted (clinical safety).
 * Values above the table maximum (1024) are returned unchanged.
 *
 * @param {number} num - Raw numeric MIC from user input.
 * @param {number|string} guidelinesId - Guidelines list item id (971 CLSI, 972 EUCAST, 0 missing).
 * @param {string} [numericString] - Original numeric portion for decimal-place inference on blur.
 * @returns {string} Rounded display MIC as a string.
 */
export function roundToValidMIC(num, guidelinesId, numericString) {
    if (!shouldApplyMicRounding(guidelinesId)) {
        return String(num);
    }
    if (num > TABLE_MAX_GEOMETRIC) {
        return String(num);
    }
    const key = guidelineColumnKey(guidelinesId);
    for (const row of MICValueMappingsForGuidelines) {
        const display = row[key];
        if (display === num || Math.abs(display - num) < FLOAT_COMPARE_EPSILON) {
            return String(display);
        }
    }
    const decimalPlaces = countDecimalPlaces(numericString);
    const artifactStep = findGeometricStepForDecimalArtifact(num, decimalPlaces);
    if (artifactStep != null) {
        return String(getGuidelineMicDisplayForGeometricStep(artifactStep, guidelinesId));
    }
    const geometricStep = geometricCeiling(num);
    return String(getGuidelineMicDisplayForGeometricStep(geometricStep, guidelinesId));
}

/**
 * Valid MIC dilution steps per guideline.
 * Geometric is the canonical doubling series used for round-up (ceiling) selection.
 * EUCAST and CLSI are display values for the same geometric step (may differ at low end).
 * Rounding on blur applies to guidelines id 971/972 (and missing id 0 via Geometric column).
 * Other specific guideline ids pass the entered value through unchanged.
 * Values above Geometric 1024 pass through uncapped; MicDosageNumber limits input to 9999.
 * Display values may use fewer decimal places than Geometric (e.g. 0.06 for 0.0625); entry such
 * as 0.063 (0.0625 at 3 dp) must snap to that step rather than ceiling to the next dilution.
 */
export const MICValueMappingsForGuidelines = [
    { Geometric: 1024, EUCAST: 1024, CLSI: 1024 },
    { Geometric: 512, EUCAST: 512, CLSI: 512 },
    { Geometric: 256, EUCAST: 256, CLSI: 256 },
    { Geometric: 128, EUCAST: 128, CLSI: 128 },
    { Geometric: 64, EUCAST: 64, CLSI: 64 },
    { Geometric: 32, EUCAST: 32, CLSI: 32 },
    { Geometric: 16, EUCAST: 16, CLSI: 16 },
    { Geometric: 8, EUCAST: 8, CLSI: 8 },
    { Geometric: 4, EUCAST: 4, CLSI: 4 },
    { Geometric: 2, EUCAST: 2, CLSI: 2 },
    { Geometric: 1, EUCAST: 1, CLSI: 1 },
    { Geometric: 0.5, EUCAST: 0.5, CLSI: 0.5 },
    { Geometric: 0.25, EUCAST: 0.25, CLSI: 0.25 },
    { Geometric: 0.125, EUCAST: 0.125, CLSI: 0.12 },
    { Geometric: 0.0625, EUCAST: 0.06, CLSI: 0.06 },
    { Geometric: 0.03125, EUCAST: 0.03, CLSI: 0.03 },
    { Geometric: 0.015625, EUCAST: 0.016, CLSI: 0.016 },
    { Geometric: 0.0078125, EUCAST: 0.008, CLSI: 0.008 },
    { Geometric: 0.00390625, EUCAST: 0.004, CLSI: 0.004 },
    { Geometric: 0.001953125, EUCAST: 0.002, CLSI: 0.002 },
    { Geometric: 0.0009765625, EUCAST: 0.001, CLSI: 0.001 }
];

/** CLSI guidelines id */
export const GUIDELINES_CLSI = 971;
/** EUCAST guidelines id */
export const GUIDELINES_EUCAST = 972;

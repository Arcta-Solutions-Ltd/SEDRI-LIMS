/** Specimen workflow states that permit isolate/direct test row actions on embedded grids. */
export const TEST_GRID_ALLOWED_SPECIMEN_STATES = new Set(['526', '527', '529', '532', '533', '535']);

/**
 * Normalizes a workflow state id to a string suitable for {@link TEST_GRID_ALLOWED_SPECIMEN_STATES} lookup.
 * @param {string|number|null|undefined} stateId - Raw workflow state id.
 * @returns {string|null} Normalized state id, or null when absent.
 */
const normalizeWorkflowStateId = (stateId) => {
    if (stateId == null || stateId === '') {
        return null;
    }
    return String(stateId);
};

/**
 * Returns the first normalized workflow state id from the supplied candidates.
 * @param {Array<string|number|null|undefined>} candidates - State ids in priority order.
 * @returns {string|null} First valid normalized id, or null when none apply.
 */
const firstNormalizedStateId = (candidates) => {
    for (const candidate of candidates) {
        const normalized = normalizeWorkflowStateId(candidate);
        if (normalized != null) {
            return normalized;
        }
    }
    return null;
};

/**
 * Resolves the specimen workflow state id used to decide whether test-grid row menus are built.
 * Prefers explicit parent specimen state, then selectedRecord, then props.stateid.
 * @param {Object|undefined|null} props - DirectTestsGrid props (or equivalent).
 * @param {React.MutableRefObject<string|number|undefined|null>} [props.specimenContextStateIdRef] - Fresh parent specimen state ref.
 * @param {string|number|undefined} [props.parentSpecimenStateId] - Parent specimen state when viewing a nested culture record.
 * @param {Object|undefined|null} [props.selectedRecord] - Parent specimen record from the record view shell.
 * @param {string|number|undefined} [props.stateid] - Workflow state passed from ManageRecord.
 * @returns {string|null} Normalized specimen workflow state id, or null when unknown.
 */
export function resolveSpecimenWorkflowStateForTestGrid(props) {
    if (props == null) {
        return null;
    }

    const refState = props.specimenContextStateIdRef?.current;

    return firstNormalizedStateId([
        refState,
        props.parentSpecimenStateId,
        props.selectedRecord?.stateid,
        props.selectedRecord?.StateId,
        props.stateid,
    ]);
}

/**
 * Determines whether the resolved specimen workflow state permits test-grid row menus.
 * @param {string|number|null|undefined} stateId - Specimen workflow state id.
 * @returns {boolean} True when row menus may be built.
 */
export function isTestGridMenuStateAllowed(stateId) {
    const normalized = normalizeWorkflowStateId(stateId);
    return normalized != null && TEST_GRID_ALLOWED_SPECIMEN_STATES.has(normalized);
}

export default resolveSpecimenWorkflowStateForTestGrid;

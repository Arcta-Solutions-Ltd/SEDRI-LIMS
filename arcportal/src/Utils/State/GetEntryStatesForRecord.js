import LaboratoryList from "../../Classes/Laboratory/LaboratoryList";

/**
 * Resolves the comma-separated workflow entry-state string for a given button in the
 * context of a specific specimen type and laboratory.
 *
 * The function looks up the laboratory's active {@code WorkflowId} from
 * {@code laboratoryList} and then finds the matching {@code WorkflowEntryStates} entry
 * on the button. The returned string is used by {@link FilterMenusOnState} and
 * {@link FilterGroupMenuOnState} to determine whether the current specimen state permits
 * the button's action.
 *
 * Returns an empty string when:
 * - The button is not workflow-controlled ({@code workflow}/{@code Workflow} is falsy).
 * - No matching {@code WorkflowEntryStates} entry is found for the resolved workflow ID.
 *
 * Property names are resolved with a camelCase-first, PascalCase-fallback strategy so
 * that the function works with items produced by {@code MapButtonsToContextMenu}
 * (camelCase) and with raw server-serialised {@code ButtonConfig} objects (PascalCase).
 *
 * @param {string|number|undefined} specimenTypeId - The specimen type identifier used to
 *   select the correct laboratory entry from {@code laboratoryList}.
 * @param {string|number} laboratoryId - The laboratory identifier whose {@code WorkflowId}
 *   is used to select the right entry from the button's {@code WorkflowEntryStates} array.
 * @param {Object|Array} laboratoryList - The laboratory configuration data, accepted in
 *   any format that {@link LaboratoryList} can consume.
 * @param {Object} button - The button or menu-item descriptor. Must expose either
 *   {@code workflow}/{@code Workflow} (boolean) and {@code entryStates}/{@code WorkflowEntryStates}
 *   (array of {@code {WorkflowId, States}} objects), or a comma-separated semantic string
 *   (e.g. test list views using {@code "edit, editanddelete"}).
 * @returns {string} A comma-separated string of state IDs (e.g. {@code "526, 527, 529"}),
 *   semantic entry states for test rows, or an empty string when the button is not
 *   workflow-controlled or no states are found.
 */
const GetEntryStatesForRecord = (specimenTypeId, laboratoryId, laboratoryList, button) => {
    
    const isUnderWorkflow = button.workflow ?? button.Workflow;
    if (! isUnderWorkflow) { return "";}

    const laboratoryListToUse = new LaboratoryList(laboratoryList, specimenTypeId);
    const laboratory = laboratoryListToUse.getLaboratory(laboratoryId);

    const workflowId = laboratory?.WorkflowId;

    const statesToUse = button.entryStates ?? button.WorkflowEntryStates;
    if (typeof statesToUse === 'string') {
        return statesToUse;
    }
    const defaultWorkflow = Array.isArray(statesToUse) ? (workflowId !== undefined ? statesToUse.find(item => item.WorkflowId.toString() === workflowId.toString()) : statesToUse[0]) : undefined;
    return defaultWorkflow?.States ?? "";
}

/**
 * Returns the workflow definition object that applies to a given specimen type and laboratory.
 *
 * Looks up the laboratory's active {@code WorkflowId} via {@link LaboratoryList} and then
 * finds the matching workflow definition from the supplied {@code workflows} array. This is
 * used by {@code ManageRecord} and related components to determine which workflow pages,
 * fields, and culture options should be visible for the current specimen.
 *
 * @param {string|number|null|undefined} specimenTypeId - The specimen type identifier. When
 *   {@code null} or an empty string the function returns {@code 0} immediately.
 * @param {string|number} laboratoryId - The laboratory identifier used to look up the
 *   {@code WorkflowId} from the laboratory configuration.
 * @param {Object|Array} laboratoryList - The laboratory configuration data accepted by
 *   {@link LaboratoryList}.
 * @param {Array<{Id: number}>} workflows - The full list of workflow definition objects
 *   available in the application. The function returns the first entry whose {@code Id}
 *   matches the laboratory's {@code WorkflowId}.
 * @returns {Object|undefined} The matching workflow definition object, or {@code undefined}
 *   when no match is found. Returns {@code 0} when {@code specimenTypeId} is absent.
 */
const GetWorkflowFromSpecimenType = (specimenTypeId, laboratoryId, laboratoryList, workflows) => {
    
    if (specimenTypeId == null || specimenTypeId === "") { return 0;}

    const laboratoryListToUse = new LaboratoryList(laboratoryList, specimenTypeId);
    const laboratory = laboratoryListToUse.getLaboratory(laboratoryId);
    const workflowId = laboratory?.WorkflowId;
    
    return workflows.find(item => item.Id === workflowId);
}

export default GetEntryStatesForRecord;

export {GetWorkflowFromSpecimenType};

import GetEntryStatesForRecord from "./GetEntryStatesForRecord";

const FilterButtonsOnState = (buttons, state, specimenTypeId, laboratoryid, laboratoryConfig) => {

    specimenTypeId ??= 1;
    if (state !== undefined) {
        buttons = buttons.filter( b => {

            if (b.Key.toLowerCase() === "addculture") {
                var x = 1;
            }

            if (b.Workflow) {
                if (Array.isArray(b.WorkflowEntryStates) && b.WorkflowEntryStates.length > 0) {
                    const entryStatesToUse = GetEntryStatesForRecord(specimenTypeId, laboratoryid, laboratoryConfig,b) ?? "";
                    const stateList = entryStatesToUse.split(",").map((item)=>item.trim());
                    return stateList.includes(state.toString());
                } else {
                    return false;
                }
            } else {
                return true;
            }
        });
    }

    return buttons;
}

export default FilterButtonsOnState;


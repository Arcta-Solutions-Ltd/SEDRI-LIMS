const IsValidEntryStateForButton = (button, state) => {

    if (button.Workflow) {
        if (button.EntryStates !== undefined) {
            let stateList = button.EntryStates.split(",").map((item)=>item.trim());
            if (stateList.length === 0) {
                return true;
            }
            if (state !== undefined) {
                let isValid = stateList.includes(state.toString());
                return isValid;
            } else {
                return false;
            }

        } else {
            return true;
        }
    } else {
        return true;
    }
}

export default IsValidEntryStateForButton;
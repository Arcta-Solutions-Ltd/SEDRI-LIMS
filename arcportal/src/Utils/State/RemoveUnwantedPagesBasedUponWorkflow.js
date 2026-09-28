import { GetWorkflowFromSpecimenType } from "./GetEntryStatesForRecord";

const RemoveUnwantedPagesBasedUponWorkflow = (pages, specimentypeid, laboratoryId, laboratory, workflow) => {

    if (specimentypeid != null && specimentypeid !== "" && laboratoryId != null && laboratoryId !== "") {
        const currentWorkflow = GetWorkflowFromSpecimenType(specimentypeid, laboratoryId, laboratory, workflow);
        const pageIndex = pages.findIndex((p) => p.Name.toLowerCase() === "culturetypeselectionpage");
        if (pageIndex > -1) {
            pages[pageIndex].Visible = currentWorkflow.IncludeCulture;
        }
    }

    return pages;
}

export default RemoveUnwantedPagesBasedUponWorkflow;
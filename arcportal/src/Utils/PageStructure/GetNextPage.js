import RemoveUnwantedPagesBasedUponWorkflow from "../State/RemoveUnwantedPagesBasedUponWorkflow";
import ChangeFieldOptionsBasedUponWorkflow from "../State/ChangeFieldOptionsBasedUponWorkflow";

const GetNextPage = (currentPage, pageStructure, specimentypeid, laboratoryList, workflow, laboratoryId, language) => {
    
    // Forms without a specimen context (e.g. instrument profiles) have null/undefined SpecimenTypeId — skip workflow lab logic.
    if (specimentypeid != null && specimentypeid !== "" && laboratoryList !== undefined && workflow !== undefined) {
        const laboratoryCopy = structuredClone(laboratoryList);
        RemoveUnwantedPagesBasedUponWorkflow(pageStructure, specimentypeid, laboratoryId, laboratoryCopy, workflow);
        ChangeFieldOptionsBasedUponWorkflow(pageStructure, specimentypeid, laboratoryId, laboratoryCopy, workflow, language);
    }

    const pagesToConsider = pageStructure.filter(p => p.Visible);
    const currentIndex = pagesToConsider.findIndex((page) => { return page.Name === currentPage.Name});
    const newPage = pagesToConsider[currentIndex+1];
    return newPage;
}

export default GetNextPage;
import { GetWorkflowFromSpecimenType } from "./GetEntryStatesForRecord";
import TranslateTag from "../Local/TranslateTag";

const ChangeFieldOptionsBasedUponWorkflow = (pages, specimentypeid, laboratoryId, laboratory, workflow, language) => {

    if (specimentypeid !== undefined && laboratoryId !== undefined) {
        const currentWorkflow = GetWorkflowFromSpecimenType(specimentypeid, laboratoryId, laboratory, workflow);
        const pageIndex = pages.findIndex((p) => p.Name.toLowerCase() === "ackreceipt");
        if (pageIndex > -1) {
            const startTestingLabel = currentWorkflow.IncludeCulture ? TranslateTag("@GenBeg@", language) : TranslateTag("@GenBegA@", language);
            pages[pageIndex].Columns[0].FormGroups[3].Fields[0].Options[1].text = startTestingLabel;
        }
    }
}

export default ChangeFieldOptionsBasedUponWorkflow;
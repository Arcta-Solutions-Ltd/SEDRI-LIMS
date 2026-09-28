import IsThisTheLastPage from "./IsThisTheLastPage";
import EvaluateRules from "../Rules/EvaluateRules";

const AreOtherPagesInvisible = (currentPage, pageStructure) => {

    if (! IsThisTheLastPage(currentPage, pageStructure)) {
        let pageFound = false;

        for (const page of pageStructure) {
            if (pageFound) {
                return ! EvaluateRules("visible", page.Rules, pageStructure);
            } else {
                pageFound = page.Name === currentPage.Name;
            }
        }
    }
    return true;
}

export default AreOtherPagesInvisible;
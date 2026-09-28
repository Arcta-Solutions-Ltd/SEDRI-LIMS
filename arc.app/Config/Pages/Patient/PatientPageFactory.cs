using arc.app.Common;
using arc.app.Config.Pages.Patient;

namespace arc.app.Config.Pages
{
    internal class PatientPageFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addpatienttagpage" => new AddPatientTagPageConfig(),
                "managepatientattachmentspage" => new ManagePatientAttachmentsPageConfig(),
                "batchaddpatienttagpage" => new BatchAddPatientTagPageConfig(),
                "deletepatientpage" => new DeletePatientPageConfig(),
                "mergepatientpage" => new MergePatientPageConfig(),
                "movepatientpage" => new MovePatientPageConfig(),
                "patientaddresspage" => new PatientAddressPageConfig(),
                "patientcommentpage" => new PatientCommentPageConfig(),
                "patientdetailspage" => new PatientDetailsPageConfig(),
                "patientsearchpage" => new PatientSearchPageConfig(),
                "patientsearchresultspage" => new PatientSearchResultsPageConfig(),
                "patientsearchresultswithnoaddpage" => new PatientSearchResultsWithNoAddPageConfig(),
                "editpatientdetailspage" => new EditPatientDetailsPageConfig(),
                _ => null,
            };
        }
    }
}

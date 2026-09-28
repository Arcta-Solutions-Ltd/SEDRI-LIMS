using arc.app.Common;
using arc.app.Config.Queries.Patient;

namespace arc.app.Config.Queries
{
    internal class PatientQueryFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addpatienttagforminitialquery" => new AddPatientTagFormInitialQuery(),
                "managepatientattachmentsforminitialquery" => new ManagePatientAttachmentsFormInitialQuery(),
                "patientattachmentsforpatientview" => new PatientAttachmentsForPatientViewQuery(),
                "commentlistbypatientid" => new CommentListByPatientIdQuery(),
                "doespatientcontainspecimenscheckquery" => new DoesPatientContainSpecimensCheckQuery(),
                "patientbyid" => new PatientByIdQuery(),
                "patientbyidformergequery" => new PatientByIdForMergeQuery(),
                "patientdiaryentryquery" => new PatientDiaryEntryQuery(),
                "patientforpatientview" => new PatientForPatientViewQuery(),
                "patientlabelavailablefields" => new PatientLabelAvailableFieldsQuery(),
                "patientlist" => new PatientListQuery(),
                "patientrefexists" => new PatientRefExistsQuery(),
                "patientsearch" => new PatientSearchQuery(),
                "singlepatientforpatientlabel" => new SinglePatientForPatientLabelQuery(),
                "singlepatientforpatientlist" => new SinglePatientForPatientListQuery(),
                _ => null,
            };
        }
    }
}

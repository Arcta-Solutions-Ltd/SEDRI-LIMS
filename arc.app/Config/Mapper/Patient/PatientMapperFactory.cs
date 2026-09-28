using arc.app.Common;
using arc.app.Config.Mapper.Patient;

namespace arc.app.Config.Mapper
{
    internal class PatientMapperFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addpatientmapper" => new AddPatientMapper(),
                "managepatientattachmentsmapper" => new ManagePatientAttachmentsMapper(),
                "patientattachmentsviewmapper" => new PatientAttachmentsViewMapper(),
                "addpatienttagmapper" => new AddPatientTagMapper(),
                "editpatientmapper" => new EditPatientMapper(),
                "patientcommentmapper" => new PatientCommentMapper(),
                "patientdiaryparametermapper" => new PatientDiaryParameterMapper(),
                "patientdiaryresultmapper" => new PatientDiaryResultMapper(),
                "patientmergemapper" => new PatientMergeMapper(),
                "patientsearchparametermapper" => new PatientSearchParameterMapper(),
                "patientviewmapper" => new PatientViewMapper(),
                "patientbyidmapper" => new PatientByIdMapper(),
                "patientlabelavailablefieldsmapper" => new PatientLabelAvailableFieldsMapper(),
                _ => null,
            };
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class PatientFormFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addpatientform" => new AddPatientFormConfig(),
                "addpatienttagform" => new AddPatientTagFormConfig(),
                "managepatientattachmentsform" => new ManagePatientAttachmentsFormConfig(),
                "batchaddpatienttagform" => new BatchAddPatientTagFormConfig(),
                "deletepatientform" => new DeletePatientFormConfig(),
                "editpatientform" => new EditPatientFormConfig(),
                "mergepatientform" => new MergePatientFormConfig(),
                "movepatientform" => new MovePatientFormConfig(),
                "patientcommentform" => new PatientCommentFormConfig(),
                _ => null,
            };
        }
    }
}

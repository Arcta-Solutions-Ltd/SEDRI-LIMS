using arc.app.Common;
using arc.app.Config.Events.Patient;

namespace arc.app.Config.Events
{
    internal class PatientEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addpatient" => new AddPatientEventConfig(),
                "managepatientattachments" => new ManagePatientAttachmentsEventConfig(),
                "addpatienttag" => new AddPatientTagEventConfig(),
                "batchaddpatienttag" => new BatchAddPatientTagEventConfig(),
                "deletepatient" => new DeletePatientEventConfig(),
                "editpatient" => new EditPatientEventConfig(),
                "mergepatient" => new MergePatientEventConfig(),
                "movepatient" => new MovePatientEventConfig(),
                "patientcomment" => new PatientCommentEventConfig(),
                _ => null,
            };
        }
    }
}

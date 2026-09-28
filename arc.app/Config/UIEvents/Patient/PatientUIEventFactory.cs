using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class PatientUIEventFactory : IDefinitionFactory
    {
        public IDefinition Create(string definitionName)
        {
            return definitionName.ToLower() switch
            {
                "addpatienttaguievent" => new AddPatientTagUIEventConfig(),
                "managepatientattachmentsuievent" => new ManagePatientAttachmentsUIEventConfig(),
                "batchaddpatienttaguievent" => new BatchAddPatientTagUIEventConfig(),
                "deletepatientuievent" => new DeletePatientUIEventConfig(),
                "editpatientuievent" => new EditPatientUIEventConfig(),
                "mergepatientuievent" => new MergePatientUIEventConfig(),
                "movepatientuievent" => new MovePatientUIEventConfig(),
                "patientcommentuievent" => new PatientCommentUIEventConfig(),
                "patientdiaryuievent" => new PatientDiaryUIEventConfig(),
                "printpatientbarcode1uievent" => new PrintPatientBarcode1UIEventConfig(),
                "printpatientbarcode2uievent" => new PrintPatientBarcode2UIEventConfig(),
                "viewpatientrecorduievent" => new ViewPatientRecordUIEventConfig(),
                _ => null,
            };
        }
    }
}

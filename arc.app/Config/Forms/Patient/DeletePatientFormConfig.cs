using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeletePatientFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletepatientform',
                        viewTitle: 'Delete an existing patient.',
                        saveEvent: 'deletepatient',
                        initialQuery: 'PatientById',
                        suppressRecordView: true,
                        pages: ['deletepatientpage']
                    }";

            return form;
        }
    }
}

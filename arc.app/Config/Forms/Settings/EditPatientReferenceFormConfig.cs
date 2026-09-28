using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditPatientReferenceFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editpatientreferenceform',
                        viewTitle: 'Edit patient reference.',
                        saveEvent: 'editpatientreference',
                        suppressRecordView: true,
                        initialQuery: 'editpatientreferencequery',
                        pages: [ 'editpatientreferencepage']
                    }";

            return form;
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddPatientReferenceTextFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addpatientreferencetextform',
                        viewTitle: 'Edit patient reference.',
                        saveEvent: 'addpatientreferencetext',
                        suppressRecordView: true,
                        pages: [ 'addpatientreferencetextpage']
                    }";

            return form;
        }
    }
}

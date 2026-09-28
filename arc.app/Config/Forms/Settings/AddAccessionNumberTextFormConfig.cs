using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddAccessionNumberTextFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addaccessionnumbertextform',
                        viewTitle: 'Edit accession number.',
                        saveEvent: 'addaccessionnumbertext',
                        suppressRecordView: true,
                        pages: [ 'addaccessionnumbertextpage']
                    }";

            return form;
        }
    }
}

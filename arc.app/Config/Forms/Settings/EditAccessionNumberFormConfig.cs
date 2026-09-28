using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditAccessionNumberFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editaccessionnumberform',
                        viewTitle: 'Edit accession number.',
                        saveEvent: 'editaccessionnumber',
                        suppressRecordView: true,
                        initialQuery: 'editaccessionnumberquery',
                        pages: [ 'editaccessionnumberpage']
                    }";

            return form;
        }
    }
}

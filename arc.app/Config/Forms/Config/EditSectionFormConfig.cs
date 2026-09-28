using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditSectionFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editsectionform',
                        viewTitle: 'Edit section',
                        saveEvent: 'editsection',
                        suppressRecordView: true,
                        initialQuery: 'editsectionquery',
                        pages: [ 'editsectionpage', 'fieldselectorpage']
                    }";

            return form;
        }
    }
}

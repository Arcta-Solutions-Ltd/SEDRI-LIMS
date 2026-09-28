using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditSettingFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editsettingform',
                        viewTitle: 'Edit setting.',
                        saveEvent: 'editsetting',
                        suppressRecordView: true,
                        initialQuery: 'editsettingquery',
                        pages: [ 'editsettingpage']
                    }";

            return form;
        }
    }
}

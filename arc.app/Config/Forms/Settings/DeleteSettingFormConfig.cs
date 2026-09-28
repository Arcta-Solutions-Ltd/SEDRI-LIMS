using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteSettingFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletesettingform',
                        viewTitle: 'Delete setting.',
                        saveEvent: 'deletesetting',
                        suppressRecordView: true,
                        initialQuery: 'deletesettingquery',
                        pages: [ 'deletesettingpage']
                    }";

            return form;
        }
    }
}

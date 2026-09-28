using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditFormFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editformform',
                        viewTitle: 'Edit an existing form.',
                        saveEvent: 'editform',
                        suppressRecordView: true,
                        pages: [ 'editformpage']
                    }";

            return form;
        }
    }
}

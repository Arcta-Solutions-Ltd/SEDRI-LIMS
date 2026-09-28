using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditHostFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'edithostform',
                        viewTitle: 'Edit a host.',
                        saveEvent: 'edithost',
                        suppressRecordView: true,
                        pages: ['edithostpage']
                    }";

            return form;
        }
    }
}

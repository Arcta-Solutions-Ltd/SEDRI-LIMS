using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddHostFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addhostform',
                        viewTitle: 'Add a new host.',
                        saveEvent: 'addhost',
                        suppressRecordView: true,
                        pages: ['addhostpage']
                    }";

            return form;
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteHostFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletehostform',
                        viewTitle: 'Delete a host.',
                        saveEvent: 'deletehost',
                        suppressRecordView: true,
                        pages: ['deletehostpage']
                    }";

            return form;
        }
    }
}

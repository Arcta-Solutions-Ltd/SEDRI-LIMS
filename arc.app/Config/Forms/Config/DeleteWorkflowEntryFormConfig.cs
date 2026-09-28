using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteWorkflowEntryFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deleteworkflowentryform',
                        viewTitle: 'Delete workflow entry.',
                        saveEvent: 'deleteworkflowentry',
                        initialQuery: 'deleteworkflowentryquery',
                        suppressRecordView: true,
                        pages: [ 'deleteworkflowentrypage']
                    }";

            return form;
        }
    }
}

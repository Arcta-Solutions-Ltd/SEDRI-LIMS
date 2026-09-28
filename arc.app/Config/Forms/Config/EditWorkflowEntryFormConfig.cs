using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditWorkflowEntryFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'editworkflowentryform',
                        viewTitle: 'Edit workflow entry.',
                        saveEvent: 'editworkflowentry',
                        initialQuery: 'editworkflowentryquery',
                        suppressRecordView: true,
                        pages: [ 'editworkflowentrypage', 'workflowentrysecondpage']
                    }";

            return form;
        }
    }
}

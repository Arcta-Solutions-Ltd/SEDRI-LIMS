using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class EditTableEntryFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'edittableentryform',
                        viewTitle: 'Edit a table entry.',
                        initialQuery: 'ListEntryByIdForEdit',
                        saveEvent: 'edittableentry',
                        suppressRecordView: true,
                        pages: ['edittableentrypage']
                    }";

            return form;
        }
    }
}

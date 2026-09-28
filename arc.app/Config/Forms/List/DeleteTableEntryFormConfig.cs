using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class DeleteTableEntryFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'deletetableentryform',
                        viewTitle: 'Delete a table entry.',
                        saveEvent: 'deletetableentry',
                        suppressRecordView: true,
                        useListData: true,
                        pages: ['deletetableentrypage']
                    }";

            return form;
        }
    }
}

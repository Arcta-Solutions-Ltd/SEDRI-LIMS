using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class AddTableEntryFormConfig : IDefinition
    {
        public string Get()
        {
            var form = @"{
                        name: 'addtableentryform',
                        viewTitle: 'Add a new table entry.',
                        initialQuery: 'ListById',
                        saveEvent: 'addtableentry',
                        suppressRecordView: true,
                        pages: ['tableentrypage']
                    }";

            return form;
        }
    }
}

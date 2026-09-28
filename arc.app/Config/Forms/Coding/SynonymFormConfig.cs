using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class SynonymFormConfig : IDefinition
    {
        public string Get()
        {
            var form =
                @"{
                    name: 'synonymform',
                    title: '@OrgManC@',
                    viewTitle: 'Manage synonyms.',
                    saveEvent: 'synonym',
                    suppressRecordView: true,
                    initialQuery: 'synonymsfororganismquery',
                    pages: ['synonympage']
                }";

            return form;
        }
    }
}

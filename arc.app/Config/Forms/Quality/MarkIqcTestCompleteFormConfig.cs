using arc.app.Common;

namespace arc.app.Config.Forms
{
    internal class MarkIqcTestCompleteFormConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'markiqctestcompleteform',
                viewTitle: '@QuaMarIqcTesCom@.',
                saveEvent: 'markiqctestcomplete',
                initialQuery: 'markiqctestcompletequery',
                suppressRecordView: true,
                pages: ['markiqctestcompletepage']
            }";
        }
    }
}

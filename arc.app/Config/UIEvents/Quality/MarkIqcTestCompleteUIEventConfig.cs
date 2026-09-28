using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class MarkIqcTestCompleteUIEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'markiqctestcompleteuievent',
                description: '@QuaMarIqcTesCom@',
                type: 'form',
                action: 'markiqctestcompleteform'
            }";
        }
    }
}

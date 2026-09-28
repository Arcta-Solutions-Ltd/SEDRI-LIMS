using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddExpertRuleConditionUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent =
                @"{
                        name: 'addexpertruleconditionuievent',
                        description: 'Add Rule Condition',
                        type: 'form',
                        action: 'addexpertruleconditionform'
                    }";

            return newEvent;
        }
    }
}

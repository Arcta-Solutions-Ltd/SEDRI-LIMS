using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditExpertRuleConditionUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent =
                @"{
                        name: 'editexpertruleconditionuievent',
                        description: 'Edit Rule Condition',
                        type: 'form',
                        action: 'editexpertruleconditionform'
                    }";

            return newEvent;
        }
    }
}

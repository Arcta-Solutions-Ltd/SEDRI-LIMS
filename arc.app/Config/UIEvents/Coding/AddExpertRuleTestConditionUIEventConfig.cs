using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddExpertRuleTestConditionUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent =
                @"{
                        name: 'addexpertruletestconditionuievent',
                        description: 'Add Rule Test Condition',
                        type: 'form',
                        action: 'addexpertruletestconditionform'
                    }";

            return newEvent;
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class ConditionSelectorUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent =
                @"{
                        name: 'conditionselectoruievent',
                        description: 'Condition Selector',
                        type: 'form',
                        action: 'conditionselectorform'
                    }";

            return newEvent;
        }
    }
}

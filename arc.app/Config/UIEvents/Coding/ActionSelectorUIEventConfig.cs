using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class ActionSelectorUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent =
                @"{
                        name: 'actionselectoruievent',
                        description: 'Action Selector',
                        type: 'form',
                        action: 'actionselectorform'
                    }";

            return newEvent;
        }
    }
}

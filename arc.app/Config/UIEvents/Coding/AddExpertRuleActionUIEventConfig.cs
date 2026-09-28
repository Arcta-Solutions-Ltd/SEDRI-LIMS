using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddExpertRuleActionUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent =
                @"{
                        name: 'addexpertruleactionuievent',
                        description: 'Add Rule Action',
                        type: 'form',
                        action: 'addexpertruleactionform'
                    }";

            return newEvent;
        }
    }
}

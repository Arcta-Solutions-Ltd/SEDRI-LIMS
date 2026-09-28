using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditExpertRuleActionUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent =
                @"{
                        name: 'editexpertruleactionuievent',
                        description: 'Edit Rule Action',
                        type: 'form',
                        action: 'editexpertruleactionform'
                    }";

            return newEvent;
        }
    }
}

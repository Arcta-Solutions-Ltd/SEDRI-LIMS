using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddExpertRuleUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addexpertruleuievent',
                        description: 'Add Expert Rule',
                        type: 'form',
                        action: 'addexpertruleform'
                    }";

            return newEvent;
        }
    }
}

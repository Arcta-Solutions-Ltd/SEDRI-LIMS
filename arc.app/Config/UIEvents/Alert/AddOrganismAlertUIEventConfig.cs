using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddOrganismAlertUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addorganismalertuievent',
                        description: 'Add Alert',
                        type: 'form',
                        action: 'addorganismalertform'
                    }";

            return newEvent;
        }
    }
}

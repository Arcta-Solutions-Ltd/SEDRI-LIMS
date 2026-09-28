using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EventPermissionsUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'eventpermissions',
                        description: 'Maintain event permissions for a role',
                        type: 'form',
                        action: 'eventpermissions'
                    }";

            return newEvent;
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class MenuPermissionsUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'menupermissions',
                        description: 'Maintain menu permissions for a role',
                        type: 'form',
                        action: 'menupermissions'
                    }";

            return newEvent;
        }
    }
}

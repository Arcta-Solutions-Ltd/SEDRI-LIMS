using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddRoleUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addrole',
                        description: 'Add a new role',
                        type: 'form',
                        action: 'addroleform'
                    }";

            return newEvent;
        }
    }
}

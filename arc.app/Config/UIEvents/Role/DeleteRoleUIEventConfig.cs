using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteRoleUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deleterole',
                        description: 'Delete a role',
                        type: 'form',
                        action: 'deleteroleform'
                    }";

            return newEvent;
        }
    }
}

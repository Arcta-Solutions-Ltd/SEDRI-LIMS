using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditRoleUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editrole',
                        description: 'Add a new role',
                        type: 'form',
                        action: 'editroleform'
                    }";

            return newEvent;
        }
    }
}

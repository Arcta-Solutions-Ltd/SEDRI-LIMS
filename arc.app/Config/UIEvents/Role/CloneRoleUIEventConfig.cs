using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class CloneRoleUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'clonerole',
                        description: 'Clone a role',
                        type: 'form',
                        action: 'cloneroleform'
                    }";

            return newEvent;
        }
    }
}

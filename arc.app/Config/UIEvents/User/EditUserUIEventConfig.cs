using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditUserUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'edituser',
                        description: 'Edit a new user',
                        type: 'form',
                        action: 'edituserform'
                    }";

            return newEvent;
        }
    }
}

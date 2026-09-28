using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class ChangePasswordUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'changepassworduievent',
                        description: 'Change Password',
                        type: 'form',
                        action: 'changepasswordform'
                    }";

            return newEvent;
        }
    }
}

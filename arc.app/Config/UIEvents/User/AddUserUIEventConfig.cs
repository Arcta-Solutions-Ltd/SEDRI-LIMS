using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddUserUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'adduser',
                        description: 'Add a new user',
                        type: 'form',
                        action: 'adduserform'
                    }";

            return newEvent;
        }
    }
}

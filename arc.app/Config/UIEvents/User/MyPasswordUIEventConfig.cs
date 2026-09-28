using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class MyPasswordUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'mypassworduievent',
                        description: 'Change My Password',
                        type: 'form',
                        action: 'mypasswordform'
                    }";

            return newEvent;
        }
    }
}

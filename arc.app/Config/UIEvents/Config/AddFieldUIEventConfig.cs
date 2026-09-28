using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddFieldUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addfielduievent',
                        description: 'Add field',
                        type: 'form',
                        action: 'addfieldform'
                    }";

            return newEvent;
        }
    }
}

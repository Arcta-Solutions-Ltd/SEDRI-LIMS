using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddStateUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addstateuievent',
                        description: 'Add state',
                        type: 'form',
                        action: 'addstateform'
                    }";

            return newEvent;
        }
    }
}

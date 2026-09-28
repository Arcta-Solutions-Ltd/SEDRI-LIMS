using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditStateUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editstateuievent',
                        description: 'Edit state',
                        type: 'form',
                        action: 'editstateform'
                    }";

            return newEvent;
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditWordUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editworduievent',
                        description: 'Edit Word',
                        type: 'form',
                        action: 'editwordform'
                    }";

            return newEvent;
        }
    }
}

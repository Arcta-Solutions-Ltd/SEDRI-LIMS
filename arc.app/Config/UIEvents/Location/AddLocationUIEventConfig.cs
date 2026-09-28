using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddLocationUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addlocationuievent',
                        description: 'Add Location',
                        type: 'form',
                        action: 'addlocationform'
                    }";

            return newEvent;
        }
    }
}

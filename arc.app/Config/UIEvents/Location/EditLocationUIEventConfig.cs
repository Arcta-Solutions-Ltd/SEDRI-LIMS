using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditLocationUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editlocationuievent',
                        description: 'Edit Location',
                        type: 'form',
                        action: 'editlocationform'
                    }";

            return newEvent;
        }
    }
}

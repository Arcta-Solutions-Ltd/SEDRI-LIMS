using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddCustomUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addcustomuievent',
                        description: 'Add Custom Entry',
                        type: 'form',
                        action: 'addcustomform'
                    }";

            return newEvent;
        }
    }
}

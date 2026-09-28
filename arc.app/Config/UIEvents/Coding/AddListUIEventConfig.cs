using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddListUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addlistuievent',
                        description: 'Add List',
                        type: 'form',
                        action: 'addlistform'
                    }";

            return newEvent;
        }
    }
}

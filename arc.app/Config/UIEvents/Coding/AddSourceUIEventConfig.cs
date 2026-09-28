using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddSourceUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addsourceuievent',
                        description: 'Add Source',
                        type: 'form',
                        action: 'addsourceform'
                    }";

            return newEvent;
        }
    }
}

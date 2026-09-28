using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddTagUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addtaguievent',
                        description: 'Add Tag',
                        type: 'form',
                        action: 'addtagform'
                    }";

            return newEvent;
        }
    }
}

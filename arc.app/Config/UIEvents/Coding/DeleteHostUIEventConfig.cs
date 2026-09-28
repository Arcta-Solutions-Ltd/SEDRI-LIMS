using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteHostUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletehostuievent',
                        description: 'Delete Host',
                        type: 'form',
                        action: 'deletehostform'
                    }";

            return newEvent;
        }
    }
}

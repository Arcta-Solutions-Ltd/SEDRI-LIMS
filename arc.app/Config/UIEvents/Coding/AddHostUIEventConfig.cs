using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddHostUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addhostuievent',
                        description: 'Add Host',
                        type: 'form',
                        action: 'addhostform'
                    }";

            return newEvent;
        }
    }
}

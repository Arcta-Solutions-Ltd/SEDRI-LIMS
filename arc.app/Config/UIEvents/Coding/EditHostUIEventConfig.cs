using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditHostUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'edithostuievent',
                        description: 'Edit Host',
                        type: 'form',
                        action: 'edithostform'
                    }";

            return newEvent;
        }
    }
}

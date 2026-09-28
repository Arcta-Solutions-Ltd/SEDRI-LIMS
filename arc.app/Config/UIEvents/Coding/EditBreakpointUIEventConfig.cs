using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class EditBreakpointUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'editbreakpointuievent',
                        description: 'Edit Breakpoint',
                        type: 'form',
                        action: 'editbreakpointform'
                    }";

            return newEvent;
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class AddBreakpointUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'addbreakpointuievent',
                        description: 'Add Breakpoint',
                        type: 'form',
                        action: 'addbreakpointform'
                    }";

            return newEvent;
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.UIEvents
{
    internal class DeleteBreakpointUIEventConfig : IDefinition
    {
        public string Get()
        {
            var newEvent = @"{
                        name: 'deletebreakpointuievent',
                        description: 'Delete Breakpoint',
                        type: 'form',
                        action: 'deletebreakpointform'
                    }";

            return newEvent;
        }
    }
}

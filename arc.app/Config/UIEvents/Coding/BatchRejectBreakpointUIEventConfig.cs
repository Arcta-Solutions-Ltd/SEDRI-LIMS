using arc.app.Common;

namespace arc.app.Config.UIEvents.Coding;

/// <summary>
/// UI event configuration for batch rejecting breakpoints from the list view.
/// </summary>
internal class BatchRejectBreakpointUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch reject breakpoint UI event.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'batchrejectbreakpointuievent',
            description: 'Batch reject breakpoints',
            type: 'form',
            action: 'batchrejectbreakpointform'
        }";
    }
}

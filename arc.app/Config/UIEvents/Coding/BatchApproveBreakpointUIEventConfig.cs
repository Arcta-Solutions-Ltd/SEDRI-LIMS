using arc.app.Common;

namespace arc.app.Config.UIEvents.Coding;

/// <summary>
/// UI event configuration for batch approving breakpoints from the list view.
/// </summary>
internal class BatchApproveBreakpointUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch approve breakpoint UI event.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'batchapprovebreakpointuievent',
            description: 'Batch approve breakpoints',
            type: 'form',
            action: 'batchapprovebreakpointform'
        }";
    }
}

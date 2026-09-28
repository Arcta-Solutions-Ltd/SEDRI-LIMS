using arc.app.Common;

namespace arc.app.Config.Events.Coding;

/// <summary>
/// Configuration for the batch approve breakpoints event.
/// </summary>
internal class BatchApproveBreakpointEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the batch approve breakpoints event configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            EventName: 'batchapprovebreakpoint',
            Description: '@BreBatC@',
            EventType: 'batch',
            Topic: 'Breakpoints',
            TableName: 'breakpoint',
            BatchEvent: 'addbreakpointapproval',
            BatchParentIdField: 'BreakpointId'
        }";
    }
}

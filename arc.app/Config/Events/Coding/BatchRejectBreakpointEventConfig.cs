using arc.app.Common;

namespace arc.app.Config.Events.Coding;

/// <summary>
/// Configuration for the batch reject breakpoints event.
/// </summary>
internal class BatchRejectBreakpointEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the batch reject breakpoints event configuration.
    /// </summary>
    public string Get()
    {
        return @"{
            EventName: 'batchrejectbreakpoint',
            Description: '@BreBatE@',
            EventType: 'batch',
            Topic: 'Breakpoints',
            TableName: 'breakpoint',
            BatchEvent: 'addbreakpointapproval',
            BatchParentIdField: 'BreakpointId'
        }";
    }
}

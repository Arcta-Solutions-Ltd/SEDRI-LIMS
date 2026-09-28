using arc.app.Common;

namespace arc.app.Config.Forms.Coding;

/// <summary>
/// Form configuration for batch rejecting breakpoints from the list view.
/// </summary>
internal class BatchRejectBreakpointFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch reject breakpoint form.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'batchrejectbreakpointform',
            viewTitle: '@BreBatE@',
            saveEvent: 'batchrejectbreakpoint',
            suppressRecordView: true,
            pages: ['batchrejectbreakpointpage']
        }";
    }
}

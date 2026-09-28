using arc.app.Common;

namespace arc.app.Config.Forms.Coding;

/// <summary>
/// Form configuration for batch approving breakpoints from the list view.
/// </summary>
internal class BatchApproveBreakpointFormConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch approve breakpoint form.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'batchapprovebreakpointform',
            viewTitle: '@BreBatC@',
            saveEvent: 'batchapprovebreakpoint',
            suppressRecordView: true,
            pages: ['batchapprovebreakpointpage']
        }";
    }
}

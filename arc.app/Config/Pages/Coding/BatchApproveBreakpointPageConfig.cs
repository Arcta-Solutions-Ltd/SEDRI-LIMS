using arc.app.Common;

namespace arc.app.Config.Pages.Coding;

/// <summary>
/// Page configuration for batch approving breakpoints from the list view.
/// </summary>
internal class BatchApproveBreakpointPageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch approve breakpoint page.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'batchapprovebreakpointpage',
            pageTitle: '@BreBatC@',
            text: '@BreBatD@',
            columns: [
                {
                    key: 'col1',
                    formGroups: [
                        {
                            key: 'fg1',
                            fields: [
                                { id: 'BreakpointId', type: 'hidden' },
                                { id: 'CodingStatusId', type: 'hidden', defaultValue: '145' }
                            ]
                        }
                    ]
                }
            ],
            nextButton: { show: true, buttonText: '@GenApp@' }
        }";
    }
}

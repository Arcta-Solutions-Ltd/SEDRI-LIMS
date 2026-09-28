using arc.app.Common;

namespace arc.app.Config.Pages.Coding;

/// <summary>
/// Page configuration for batch rejecting breakpoints from the list view.
/// </summary>
internal class BatchRejectBreakpointPageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the batch reject breakpoint page.
    /// </summary>
    public string Get()
    {
        return @"{
            name: 'batchrejectbreakpointpage',
            pageTitle: '@BreBatE@',
            text: '@BreBatF@',
            columns: [
                {
                    key: 'col1',
                    formGroups: [
                        {
                            key: 'fg1',
                            fields: [
                                { id: 'BreakpointId', type: 'hidden' },
                                { id: 'CodingStatusId', type: 'hidden', defaultValue: '146' }
                            ]
                        }
                    ]
                }
            ],
            nextButton: { show: true, buttonText: '@GenRej@' }
        }";
    }
}

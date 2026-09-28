using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Provides the JSON definition for the 'Batch Approve Report' page.
/// </summary>
internal class BatchApproveReportPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the page definition as a JSON-formatted string.
    /// </summary>
    /// <returns>The JSON configuration for the batch approve report page.</returns>
    public string Get()
    {
        var page = @"{
            name: 'batchapprovereportpage',
            pageTitle: '@RepAppD@',
            text: '@RepAppE@.',
            columns: [
                {
                    key: 'col1',
                    fieldWidth: 'wide',
                    itemWidth: 'wide',
                    formGroups: [
                        {
                            key: 'fg1',
                            fields: [
                            ]
                        }
                    ]
                }
            ],
            nextButton: { show: true, buttonText: '@GenApp@' }
        }";

        return page;
    }
}

using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Provides the configuration for the Approve Report page.
/// </summary>
internal class ApproveReportPageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the Approve Report page configuration.
    /// </summary>
    /// <returns>A JSON string defining the page configuration.</returns>
    public string Get()
    {
        var page = @"{
            name: 'approvereportpage',
            pageTitle: '@RepAppA@',
            text: '@RepAppB@.',
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

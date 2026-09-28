using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Provides the configuration for the Unapprove Report page.
/// </summary>
internal class UnapproveReportPageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the Unapprove Report page configuration.
    /// </summary>
    /// <returns>A JSON string defining the page configuration.</returns>
    public string Get()
    {
        var page = @"{
            name: 'unapprovereportpage',
            pageTitle: '@RepUna@',
            text: '@RepUnaA@.',
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
            nextButton: { show: true, buttonText: '@GenRej@' }
        }";

        return page;
    }
}

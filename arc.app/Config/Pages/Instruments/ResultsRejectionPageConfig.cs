using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Represents the configuration for the Results Rejection page.
/// </summary>
internal class ResultsRejectionPageConfig : IDefinition
{
    /// <summary>
    /// Builds and returns a JSON string representing the configuration
    /// of the Results Rejection page, including its name, title, text,
    /// and layout details such as columns, form groups, and fields.
    /// </summary>
    /// <returns>A JSON-formatted string containing page configuration details.</returns>
    public string Get()
    {
        var page = @"{
                            name: 'resultsrejection',
                            pageTitle: '@InsRej@',
                            text: '@InsRejA@.',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'AccessionNumber', type: 'text', label: '@SpeAcc@' }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}


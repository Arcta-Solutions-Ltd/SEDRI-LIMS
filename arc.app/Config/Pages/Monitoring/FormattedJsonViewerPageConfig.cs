using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Represents the configuration for a formatted JSON viewer page.
/// </summary>
internal class FormattedJsonViewerPageConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON string representation of the page configuration.
    /// </summary>
    /// <returns>
    /// A JSON string containing the page configuration, including its name,
    /// title, text, button configurations, and crafted status.
    /// </returns>
    public string Get()
    {
        var page = @"{ 
                            name: 'formattedjsonviewer', 
                            pageTitle: '@MonVieC@', 
                            text: '@MonVieD@.', 
                            crafted: true, 
                            nextButton: { show: false }, 
                            cancelButton: { buttonText: '@GenClo@' } 
                        }";

        return page;
    }
}


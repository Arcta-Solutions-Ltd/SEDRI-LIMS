using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Provides the configuration for the Batch Print page.
/// </summary>
internal class BatchPrintPageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the Batch Print page configuration.
    /// </summary>
    /// <returns>A JSON string defining the page configuration.</returns>
    public string Get()
    {
        var page = @"{ 
            name: 'batchprintpage',
            pageTitle: '@RepBat@',
            text: '@RepPriB@.',
            crafted: true,
            nextButton: { show: false },
            cancelButton: { buttonText: '@GenClo@' }
        }";

        return page;
    }
}


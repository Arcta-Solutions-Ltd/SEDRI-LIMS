using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Provides the JSON configuration for the Preference Page used in user settings.
/// This configuration defines the layout of the page including its name, title, descriptive text,
/// and the structural organization into columns and form groups. The fields within the form groups
/// represent toggle options for various user preferences.
/// </summary>
internal class PreferencePageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the JSON configuration string for the Preference Page.
    /// </summary>
    /// <returns>
    /// A JSON string that specifies the page layout, including the page's name, title, text,
    /// columns, form groups, and toggle fields for preferences.
    /// </returns>
    public string Get()
    {
        // JSON configuration for the preference page layout.
        var page = @"{ 
                        name: 'preferencepage',
                        pageTitle: '@UseMyA@', 
                        text: '@UseChaB@.',
                        columns: [
                            { 
                                key: 'col1',
                                formGroups: [
                                    { 
                                        key: 'fg1',
                                        fields: [
                                            { id: 'DataFullScreen', type: 'toggle', label: '@UseDis@', required: true},
                                            { id: 'ASTRowRemovalConfirmation', type: 'toggle', label: '@AstRow@', required: true}
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}

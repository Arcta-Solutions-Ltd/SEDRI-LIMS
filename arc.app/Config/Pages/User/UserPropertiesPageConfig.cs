using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Provides the JSON configuration for the User Properties page.
/// This configuration defines the properties of the page including its name, title, descriptive text,
/// and layout details such as columns and form groups for editing user-related information (e.g., Laboratory and Organisation selections).
/// </summary>
internal class UserPropertiesPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the JSON configuration string for the User Properties page.
    /// </summary>
    /// <returns>
    /// A JSON string that defines the page layout, including the page name, title, text, columns,
    /// form groups, and fields with their respective settings.
    /// </returns>
    public string Get()
    {
        var page = @"{ 
                        name: 'userproperties',
                        pageTitle: '@UseUseC@', 
                        text: '@UseAddB@.',
                        columns: [
                            { 
                                key: 'col1',
                                formGroups: [
                                    { 
                                        key: 'fg1',
                                        fields: [
                                            { id: 'LaboratoryId', type: 'combobox', label: '@GenLabA@', required: false, multiSelect: true, placeholder: '@UseSel@', optionsName: 'LaboratoryListUnfiltered', dynamic: true }
                                        ]
                                    },
                                    { 
                                        key: 'fg2',
                                        fields: [
                                            { id: 'OrganisationId', type: 'combobox', label: '@GenOrgC@', required: false, multiSelect: true, placeholder: '@UseSelA@', optionsName: 'OrganisationListUnfiltered', dynamic: true }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}

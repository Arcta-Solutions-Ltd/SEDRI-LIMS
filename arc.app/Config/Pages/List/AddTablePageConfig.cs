using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Provides the configuration definition for the "Add Table" page,
/// including layout, fields, and metadata used in dynamic UI rendering.
/// </summary>
internal class AddTablePageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string that defines the structure and content of the "Add Table" page.
    /// This includes page metadata, column layout, form groups, and input fields.
    /// </summary>
    /// <returns>
    /// A JSON string representing the page configuration, including:
    /// - Page name and title
    /// - Column layout with field widths
    /// - Form groups containing input fields for Description (displayed as Name) and ParentId
    /// </returns>
    public string Get()
    {
        var page = @"{ 
                        name: 'addtablepage',
                        pageTitle: '@TabAddC@',
                        text: '@TabAddE@.',
                        columns: [
                            { 
                                key: 'col1',
                                fieldWidth: 'wide',
                                itemWidth: 'wide',
                                formGroups: [
                                    { 
                                        key: 'fg1',
                                        fields: [
                                            { id: 'Description', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@TabEntB@' },
                                            { id: 'ParentId', type: 'combobox', label: '@GenPar@', required: false, placeholder: '@TabEnt@', optionsName: 'CommonList', multiselect: true, dynamic: true }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }";

        return page;
    }
}

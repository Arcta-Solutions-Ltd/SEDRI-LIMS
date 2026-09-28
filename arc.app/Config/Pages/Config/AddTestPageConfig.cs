using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Add Test" page.
/// </summary>
internal class AddTestPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Add Test" page.
    /// </summary>
    /// <remarks>
    /// This configuration specifies the layout and attributes for the "Add Test" page, 
    /// including its name, title, descriptive text, and column-based structure. Each column 
    /// contains form groups with specific fields to guide user input. Fields include:
    /// - A combobox for selecting test configurations, marked as required and featuring dynamic options.
    /// - Single-line text fields for entering the test title and description, both marked as required.
    /// The configuration ensures user-friendly features like placeholders and limits on input length.
    /// </remarks>
    /// <returns>
    /// A string representation of the page configuration, detailing its structure, columns, 
    /// form groups, and fields.
    /// </returns>
    public string Get()
    {
        var page = @"{  
                            name: 'addtestpage',
                            pageTitle: '@ConAddG@',
                            text: '@ConAddH@',
                            columns: [
                                { 
                                    key: 'col1',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'TestToCloneId', type: 'combobox', label: '@ConTes@', required: true, multiselect: false, placeholder: '@ConSel@', optionsName: 'TestConfigList', tab: true, dynamic: true, translateoptions: true },
                                                { id: 'title', type: 'singleline', label: '@GenTit@', required: true, placeholder: '@ConDirC@', max:30 },
                                                { id: 'description', type: 'singleline', label: '@GenDes@', required: true, placeholder: '@ConDirD@' }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}

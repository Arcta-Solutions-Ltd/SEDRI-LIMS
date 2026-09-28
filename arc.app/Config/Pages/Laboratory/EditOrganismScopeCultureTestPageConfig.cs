using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Edit Organism Scope Culture Test" page.
/// Displays organism scope (read-only) and allows editing the associated isolate test list.
/// </summary>
internal class EditOrganismScopeCultureTestPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Edit Organism Scope Culture Test" page.
    /// </summary>
    /// <returns>
    /// A string representation of the page configuration.
    /// </returns>
    public string Get()
    {
        var page = @"{
                            name: 'editorganismscopeculturetestpage',
                            pageTitle: '@LabOrgB@',
                            text: '@LabAD@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'GroupDescription', type: 'text', label: '@GenSelI@', required: false, multiselect: false, placeholder: '@GenSelI@', tab: true },
                                                { id: 'AssociatedListId', type: 'combobox', label: '@SpeCulC@', required: true, multiselect: true, placeholder: '@SpeCulC@', optionsName: 'culturetestconfiglist', tab: true },
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}

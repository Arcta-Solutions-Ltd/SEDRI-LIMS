using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Delete Organism Scope Culture Test" page.
/// Displays organism scope and isolate tests for confirmation before delete.
/// </summary>
internal class DeleteOrganismScopeCultureTestPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Organism Scope Culture Test" page.
    /// </summary>
    /// <returns>
    /// A string representation of the page configuration.
    /// </returns>
    public string Get()
    {
        var page = @"{
                            name: 'deleteorganismscopeculturetestpage',
                            pageTitle: '@GenDelC@',
                            text: '@LabAD@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'GroupDescription', type: 'text', label: '@GenSelI@', required: false, multiselect: false, placeholder: '@GenSelI@', tab: true },
                                                { id: 'AssociatedList', type: 'text', label: '@SpeCulC@', required: false, multiselect: false, placeholder: '@SpeCulC@', tab: true },
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}

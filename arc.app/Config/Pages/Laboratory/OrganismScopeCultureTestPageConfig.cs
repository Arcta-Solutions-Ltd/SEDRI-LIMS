using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Configuration for the "Organism Scope Culture Test" page (add flow).
/// Displays after editorganismscopepage; user selects which isolate tests are available for the chosen organism scope.
/// </summary>
internal class OrganismScopeCultureTestPageConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Organism Scope Culture Test" page.
    /// </summary>
    /// <returns>
    /// A string representation of the page configuration.
    /// </returns>
    public string Get()
    {
        var page = @"{
                            name: 'organismscopeculturetestpage',
                            pageTitle: '@LabOrgB@',
                            text: '@ConAddAA@',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
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

using arc.app.Common;

namespace arc.app.Config.Pages.Specimen;

/// <summary>
/// Defines the configuration for the isolate page displaying specimen growth details.
/// Implements <see cref="IDefinition"/> to provide a JSON-formatted page layout.
/// </summary>
internal class SpecimenGrowthDetailsForIsolatePageConfig : IDefinition
{
    /// <summary>
    /// Returns the page configuration as a JSON-formatted string.
    /// The layout includes dynamic labels, conditional rules, form groups, and navigation logic.
    /// </summary>
    /// <returns>
    /// A JSON string representing the isolate page configuration for specimen growth details.
    /// </returns>
    public string Get()
    {
        var page = @"{ 
                    name: 'specimengrowthdetailsforisolatepage',
                    pageTitle: '@CulIso@',
                    text: '@CulAdd@.',
                    nextButton: { onclickstate: { state: 'organismselect',
                                                    rules:[{ effect: 'organismselect', field: 'growthTypeParentId', rule: 'isnotempty' },
                                                            { effect: 'organismselect', field: 'growthTypeParentId', rule: '!=', value: '428' }
                                                        ]
                                                },
                                    buttontext: '@GenNex@',
                                    show: true
                                },
                    required: '',
                    requiredRule: 'and',
                    columns: [
                        {
                            key: 'col1',
                            formGroups: [
                                {
                                    key: 'fg3',
                                    fields: [
                                        { id: 'ManufacturersBarcode', type: 'singleline', label: '@InsManD@', required: false, placeholder: '@InsSca@', Max: 30 },
                                    ]
                                },
                                {
                                    key: 'fg4',
                                    rules: [
                                        { effect: 'visible', field: 'growthTypeParentId', rule: '=', value: '427' }
                                    ],
                                    fields: [
                                        { id: 'Quantity', type: 'combobox', label: '@GenQua@', required: false, placeholder: '@SpeSelP@', optionsName: 'SpecimenQuantity' }
                                    ]
                                }
                            ]
                        }
                    ]
                }";

        return page;
    }
}

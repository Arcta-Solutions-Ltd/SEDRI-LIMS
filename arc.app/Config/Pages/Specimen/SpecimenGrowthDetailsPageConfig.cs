using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Provides the configuration definition for the 'specimengrowthdetails' page,
/// which captures growth-related data for a specimen within the culture workflow.
/// Define Page Contents allows add/edit/delete at page level; <c>CultureType</c> and <c>growthid</c>
/// remain locked via <c>Configurable: 'No'</c>.
/// </summary>
internal class SpecimenGrowthDetailsPageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string representing the page configuration for specimen growth details.
    /// </summary>
    /// <returns>
    /// A JSON string containing layout, fields, rules, and navigation logic for the specimen growth details page.
    /// </returns>
    public string Get()
    {
        var page = @"{ 
                            name: 'specimengrowthdetails',
                            pageTitle: '@SpeGroB@',
                            text: '@SpeProK@.',
                            configureActions: 'add,edit,delete',
                            tablename: 'culture',
                            nextButton: { onclickstate: { state: 'organismselect',
                                                          rules:[{ effect: 'organismselect', field: 'growthTypeParentId', rule: 'isnotempty' },
                                                                 { effect: 'organismselect', field: 'growthTypeParentId', rule: '!=', value: '428' }
                                                                ]
                                                        },
                                          buttontext: '@GenNex@',
                                          show: true
                                        },
                            required: 'CultureType',
                            requiredRule: 'and',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'CultureType', type: 'dropdown', label: '@CulTyp@', optionsName: 'CultureType', required: true, defaultValue: '978', Configurable: 'No' }
                                            ]
                                        },
                                        {
                                            key: 'fg2',
                                            rules:[
                                               { effect: 'visible', field: 'specimentypeid', rule: '=', value: '808'}
                                           ],
                                           fields:[
                                               { id: 'CultureBottleWeight', type: 'number', label: '@SpeBot@', required: false },
                                               { id: 'CultureBloodAndBottleWeight', type: 'number', label: '@SpeBlo@', required: false }
                                           ]
                                        },
                                        {
                                            key: 'fg3',
                                            fields: [
                                                { id: 'ManufacturersBarcode', type: 'singleline', label: '@InsManD@', required: false, placeholder: '@InsSca@', Max: 30 },
                                            ]
                                        },
                                        {
                                            key: 'fg4',
                                            fields: [
                                                { id: 'growthid', type: 'hierarchicalpicker', label: '@SpeGroC@', required: false, placeholder: '@SpeSelH@', optionsName: 'SpecimenGrowth', leavesOnly: true, Configurable: 'No' }
                                            ]
                                        },
                                        {
                                            key: 'fg5',
                                            rules:[
                                               { effect: 'visible', field: 'growthTypeParentId', rule: '=', value: '427'}
                                            ],
                                            fields: [
                                                { id: 'Quantity', type: 'combobox', label: '@GenQua@', required: false, placeholder: '@SpeSelP@', optionsName: 'SpecimenQuantity' }
                                            ]
                                        },
                                        {
                                            key: 'fg6',
                                            fields: [
                                                { id: 'PositiveDate', type: 'date', label: '@SpePosA@', required: false, placeholder: '@SpeEntG@', Min: 'now d-300', Max: 'now'},
                                                { id: 'PositiveTime', type: 'time', label: '@SpePosB@', required: false, placeholder: '@SpeEntH@', mask: '99:99'}
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}



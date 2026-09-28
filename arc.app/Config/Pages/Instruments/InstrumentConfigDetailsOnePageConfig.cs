using arc.app.Common;

namespace arc.app.Config.Pages
{
    /// <summary>
    /// Configuration for the shared “data loading rules” wizard step (second page) when adding or editing
    /// an instrument profile — interface type, approval, and conditional loading options. Not used for delete.
    /// Default Growth uses the SpecimenGrowth list (hierarchical picker, leaf selection), consistent with culture growth fields elsewhere.
    /// When the interface type is Custom (InstrumentEvent list item id 10) a required Export Profile picker
    /// (exportprofilelist) is shown; once a profile is selected an interface criteria grid (crafted, keyed on
    /// export profile field ids) and an Interface combination rule (andor, defaulting to And / list item 987) appear.
    /// </summary>
    internal class InstrumentConfigDetailsOnePageConfig : IDefinition
    {
        /// <summary>
        /// Generates the JSON for the instrumentconfigdetailsonepage definition (add/edit profile step 2).
        /// The DefaultGrowth field is a hierarchical picker bound to SpecimenGrowth (list 133), not SpecimenQuantity.
        /// </summary>
        /// <returns>A JSON string that represents the page configuration.</returns>
        public string Get()
        {
            var page = @"{
                            name: 'instrumentconfigdetailsonepage',
                            pageTitle: '@InsLoa@',
                            text: '@InsLoaB@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'InterfaceTypeId', type: 'combobox', label: '@InsInsD@', required: true, placeholder: '@InsInsD@', optionsName: 'InstrumentEvent' },
                                                { id: 'NeedsApproval', type: 'toggle', label: '@InsNee@', required: false, defaultValue: 'No' }
                                            ]
                                        },
                                        { 
                                            key: 'fgCustomProfile',
                                            rules:[{ effect: 'visible', field: 'InterfaceTypeId', rule: '=', value: 10}],
                                            fields: [
                                                { id: 'ExportProfileId', type: 'combobox', label: '@InsCusExp@', required: true, placeholder: '@InsCusExp@', optionsName: 'exportprofilelist', dynamic: true, includeFixed: true, multiSelect: false }
                                            ]
                                        },
                                        { 
                                            key: 'fgCustomCriteria',
                                            rules:[{ effect: 'visible', field: 'InterfaceTypeId', rule: '=', value: 10},
                                                   { effect: 'visible', field: 'ExportProfileId', rule: 'isnotempty' }
                                            ],
                                            fields: [
                                                { id: 'InterfaceCriteria', type: 'crafted', label: '@InsCriG@', gridfields: [
                                                        { id: 'Field', type: 'dropdown', dynamic: true },
                                                        { id: 'Comparison', type: 'dropdown', optionsName: 'comparison', width: 'small' },
                                                        { id: 'StringValue', type: 'singleline', width: 'medium', Max: 20 },
                                                        { id: 'NumberValue', type: 'number', width: 'medium' },
                                                        { id: 'ListValue', type: 'dropdown', width: 'medium' }
                                                    ]
                                                },
                                                { id: 'InterfaceCriteriaAndOr', type: 'dropdown', optionsName: 'andor', label: '@InsCriR@', width: 'narrow', defaultValue: '987' }
                                            ]
                                        },
                                        { 
                                            key: 'fg2',
                                            rules:[{ effect: 'visible', field: 'InterfaceTypeId', rule: '=', value: 9}],
                                            fields: [
                                                { id: 'DefaultGrowth', type: 'hierarchicalpicker', label: '@GenDef@', required: true, placeholder: '@InsDef@', optionsName: 'SpecimenGrowth', leavesOnly: true }
                                            ]
                                        },
                                        { 
                                            key: 'fg3',
                                            rules:[{ effect: 'visible', field: 'InterfaceTypeId', rule: '=', value: 9}],
                                            fields: [
                                                { id: 'OrganismGroupId', type: 'combobox', label: '@GenOrgE@', required: true, placeholder: '@GenOrgD@', optionsName: 'Coding', multiSelect: false, includeFixed: false, dynamic: true },
                                                { id: 'AllowIdOverwrite', type: 'toggle', label: '@InsAll@', required: false, defaultValue: 'Yes' },
                                                { id: 'AllowAstOverwrite', type: 'toggle', label: '@InsAllA@', required: false, defaultValue: 'Yes' },
                                                { id: 'AntibioticGroupId', type: 'combobox', label: '@InsAnt@', required: true, placeholder: '@GenCodB@', optionsName: 'AntibioticGroup', dynamic: true, includeFixed: false, multiSelect: false },
                                                { id: 'IgnoreUnrecognisedAntibiotics', type: 'toggle', label: '@InsIgn@', required: false, defaultValue: 'Yes' }
                                            ]
                                        },
                                        { 
                                            key: 'fg4',
                                            fields: [
                                                { id: 'IsEnabled', type: 'toggle', label: '@GenEna@', required: false, defaultValue: 'Yes' }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

            return page;
        }
    }
}


using arc.app.Common;

namespace arc.app.Config.Pages.ExpertRules;
internal class EditExpertRuleConditionPageConfig : IDefinition

{
    public string Get()
    {
        var page = @"{ 
                            name: 'editexpertruleconditionpage',
                            pageTitle: '@GenRulL@',
                            text: '@GenRulM@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            rules: [{ effect: 'visible', field: 'antibioticgroupid', rule: 'isempty'}],
                                            fields: [
                                                 { id: 'antibioticid', type: 'combobox', label: '@GenAnt@', optionsName: 'antibiotic'},
                                            ]
                                        },
                                        { 
                                            key: 'fg2',
                                            rules: [{ effect: 'visible', field: 'antibioticid', rule: 'isempty'}],
                                            fields: [
                                                 { id: 'antibioticgroupid', type: 'combobox', label: '@InsAnt@', optionsName: 'antibioticgroup'},
                                            ]
                                        },
                                        { 
                                            key: 'fg3',
                                            fields: [
                                                 { id: 'SusceptibilityId', type: 'dropdown', label: '@GenSus@', optionsName: 'testresult'},
                                                 { id: 'testmethodid', type: 'dropdown', label: '@BreTes@', optionsName: 'testmethod'},
                                                 { id: 'SpecialConsiderationId', type: 'dropdown', label: '@BreSpeB@', optionsName: 'specialconsiderations'},
                                                 { id: 'startval', type: 'number', label: '@GenStaC@', Min: '0', Max: '9999', MaxDPs: '3' },
                                                 { id: 'endval', type: 'number', label: '@GenEndA@', Min: '0', Max: '9999', MaxDPs: '3' }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}

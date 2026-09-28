using arc.app.Common;

namespace arc.app.Config.Pages.ExpertRules;

/// <summary>
/// Page configuration for the Add Expert Rule Action form.
/// Uses mutually exclusive visibility rules: antibiotic field visible when antibiotic group is empty;
/// antibiotic group field visible when antibiotic is empty.
/// DisplayOnReport is a tri-state dropdown (Not set / Yes / No) with no default, so an action can be saved
/// with Print On Report left unset (empty -> NULL). An unset value renders an editable toggle (defaulting
/// to Yes) on the AST screen; selecting 'Not set' again clears a previously chosen Yes/No back to unset.
/// </summary>
internal class AddExpertRuleActionPageConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON page configuration for the add expert rule action page.
    /// </summary>
    /// <returns>JSON string defining the page structure, columns, and form groups.</returns>
    public string Get()
    {
        var page = @"{ 
                            name: 'addexpertruleactionpage',
                            pageTitle: '@GenRulN@',
                            text: '@GenRulO@.',
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
                                                 { id: 'DisplayOnReport', type: 'dropdown', label: '@GenDis@', Configurable: 'No', options: [{ key: '', text: '@GenNon@' }, { key: 'Yes', text: '@GenYesA@' }, { key: 'No', text: '@GenNo@' }]}
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}

using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class ASTPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'ast',
                            pageTitle: '@AstAnt@',
                            text: '@AstEnt@.',
                            wide: true,
                            crafted: true,
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'ESBL', type: 'dropdown', optionsName: 'ESBL' },
                                                { id: 'BL', type: 'dropdown', optionsName: 'Betalactamase' },
                                                { id: 'Carbapenemase', type: 'dropdown', optionsName: 'Carbapenemase' },
                                                { id: 'TestPattern', type: 'dropdown', optionsName: 'TestPattern' },
                                                { id: 'Susceptibility', type: 'dropdown', optionsName: 'testresult' },
                                                { id: 'Antibiotic', type: 'dropdown', optionsName: 'Antibiotic' },
                                                { id: 'Guidelines', type: 'dropdown', optionsName: 'guidelines' },
                                                { id: 'TestPatternFullList', type: 'dropdown', optionsName: 'testpatternnameslist' },
                                                { id: 'ASTCommentOne', type: 'dropdown', optionsName: 'astcannedcomments' },
                                                { id: 'ASTCommentTwo', type: 'dropdown', optionsName: 'astcannedcomments' },
                                                { id: 'AstSusceptibilityOverrideCanned', type: 'combobox', optionsName: 'astsusceptibilityoverridecannedcommentslist' },
                                                { id: 'TestPatternFullList', type: 'dropdown', optionsName: 'testpatternnameslist' },
                                                { id: 'DrugCategory', type: 'dropdown', optionsName: 'drugcategory'},
                                                { id: 'specialconsiderid', type: 'combobox', label: '@BreSpe@', optionsName: 'specialconsiderations', dynamic: false, defaultValue: '973' }
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

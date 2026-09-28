using arc.app.Common;

namespace arc.app.Config.Pages.ExpertRules
{
    internal class ExpertRuleDetailsPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'expertruledetailspage',
                            pageTitle: '@RulAddA@',
                            text: '@RulManA@.',
                            columns: [
                                {
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'ExpertRuleName', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@RulExp@', Max: 100 },
                                                { id: 'RuleText', type: 'multiline', label: '@GenDes@', required: true, placeholder: '@GenDes@', Max: 10000 },
                                                { id: 'SpecificationId', type: 'dropdown', label: '@BreSpf@', optionsName: 'specification', dynamic: true, required: false }                                             
                                            ]
                                        },
                                        {
                                            key: 'fg2',
                                            rules: [{ effect: 'visible', field: 'SpecimenTypesToExclude', rule: 'isempty'}],
                                            fields: [
                                                { id: 'SpecimenTypesToInclude', type: 'combobox', label: '@SpeSelN@', optionsName: 'SpecimenType', multiselect: true, required: false }                                               
                                            ]
                                        },
                                        {
                                            key: 'fg3',
                                            rules: [{ effect: 'visible', field: 'SpecimenTypesToInclude', rule: 'isempty'}],
                                            fields: [
                                                { id: 'SpecimenTypesToExclude', type: 'combobox', label: '@SpeSelO@', optionsName: 'SpecimenType', multiselect: true, required: false }
                                            ]
                                        },
                                    ]
                                }
                            ]
                        }";

            return page;
        }
    }
}
using arc.app.Common;

namespace arc.app.Config.Pages.ExpertRules
{
    /// <summary>
    /// Page configuration for the first page of the Edit Expert Rule form.
    /// Provides edit-specific PageTitle and Text while sharing the same field layout as the Add form.
    /// </summary>
    internal class EditExpertRuleDetailsPageConfig : IDefinition
    {
        /// <summary>
        /// Returns the JSON page configuration for the edit expert rule details page.
        /// </summary>
        /// <returns>JSON string defining the page structure, columns, and form groups.</returns>
        public string Get()
        {
            var page = @"{ 
                            name: 'editexpertruledetailspage',
                            pageTitle: '@RulEdiA@',
                            text: '@RulEdiB@.',
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
                                                //{ id: 'RuleCategoryId', type: 'combobox', label: '@GenCat@', optionsName: 'expertrulecategory', required: true  },
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

                                        {
                                            key: 'fg4',
                                            fields: [
                                                { id: 'Enabled', type: 'toggle', label: '@GenEna@'},
                                                { id: 'AlertOnRule', type: 'toggle', label: '@RulAle@'}
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

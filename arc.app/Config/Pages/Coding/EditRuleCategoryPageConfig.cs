using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditRuleCategoryPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'editrulecategorypage',
                            pageTitle: '@RulCatC@',
                            text: '@RulCatF@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'categoryid', type: 'dropdown', label: '@RulCat@', optionsName: 'expertrulecategory', dynamic: true},
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@GenLisA@', Max: 100 }
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

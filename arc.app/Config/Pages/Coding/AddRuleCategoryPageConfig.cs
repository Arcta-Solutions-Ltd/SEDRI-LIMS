using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddRuleCategoryPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'addrulecategorypage',
                            pageTitle: '@RulCatB@',
                            text: '@RulCatE@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
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

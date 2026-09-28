using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteListPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deletelistpage',
                            pageTitle: '@CodDelE@',
                            text: '@CodDelA@.',
                            required: 'Name',
                            requiredRule: 'and',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Id', type: 'dropdown', label: '@GenNam@', required: true, placeholder: '@GenGroA@', optionsName: 'Coding', dynamic: true, removeFixed: true }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenDelC@' }
                        }";

            return page;
        }
    }
}

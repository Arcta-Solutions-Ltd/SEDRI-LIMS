using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteSourcePageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deletesourcepage',
                            pageTitle: '@CodDelF@',
                            text: '@CodDelG@.',
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
                                                { id: 'Id', type: 'dropdown', label: '@GenNam@', required: true, placeholder: '@CodSou@', optionsName: 'Guidelines', dynamic: true, removeFixed: true }
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

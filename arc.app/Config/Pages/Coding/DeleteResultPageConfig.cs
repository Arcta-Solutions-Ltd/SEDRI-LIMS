using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteResultPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deleteresultpage',
                            pageTitle: '@BreDelE@',
                            text: '@BreDelF@.',
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
                                                { id: 'Id', type: 'dropdown', label: '@GenNam@', required: true, placeholder: '@GenLisA@', optionsName: 'TestResult', dynamic: true, removeFixed: true }
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

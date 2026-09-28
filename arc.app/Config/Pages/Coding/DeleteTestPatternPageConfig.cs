using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteTestPatternPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'deletetestpatternpage',
                            pageTitle: '@TesDelA@',
                            text: '@TesDelB@.',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'testpatternname', type: 'text', label: '@GenNam@', required: true}
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

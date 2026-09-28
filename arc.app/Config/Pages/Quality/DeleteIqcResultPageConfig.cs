using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteIqcResultPageConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                name: 'deleteiqcresultpage',
                pageTitle: '@QuaDelIqcTesRes@',
                text: '@QuaDelIqcResDes@.',
                required: 'Id',
                requiredRule: 'and',
                columns: [
                    { 
                        key: 'col1',
                        formGroups: [
                            { 
                                key: 'fg1',
                                fields: [
                                    { id: 'antibioticname', type: 'text', label: '@GenNam@'},
                                    { id: 'value', type: 'text', label: '@GenVal@'}
                                ]
                            }
                        ]
                    }
                ],
                nextButton: { show: true, buttonText: '@GenDelC@' }
            }";
        }
    }
}

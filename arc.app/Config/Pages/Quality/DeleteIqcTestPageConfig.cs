using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteIqcTestPageConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                name: 'deleteiqctestpage',
                pageTitle: '@QuaDelTes@',
                text: '@QuaDelTesA@.',
                columns: [
                    { 
                        key: 'col1',
                        fieldWidth: 'wide',
                        itemWidth: 'wide',
                        formGroups: [
                            { 
                                key: 'fg1',
                                fields: [
                                    { id: 'accessionnumber', type: 'text', label: '@GenNam@'},
                                    { id: 'createddate', type: 'text', label: '@GenDatB@'},
                                    { id: 'completeddate', type: 'text', label: '@GenDatA@'},
                                    { id: 'state', type: 'text', label: '@GenStaA@'}
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

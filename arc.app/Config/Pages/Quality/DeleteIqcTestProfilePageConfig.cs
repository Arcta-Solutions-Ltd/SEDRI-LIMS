using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class DeleteIqcTestProfilePageConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                name: 'deleteiqctestprofilepage',
                pageTitle: '@QuaDelProA@',
                text: '@QuaDelProB@.',
                columns: [
                    { 
                        key: 'col1',
                        fieldWidth: 'wide',
                        itemWidth: 'wide',
                        formGroups: [
                            { 
                                key: 'fg1',
                                fields: [
                                    { id: 'Id', type: 'dropdown', label: '@GenNam@', required: true, placeholder: '@QuaIqcTesProNam@', optionsName: 'IqcTestProfiles', dynamic: true, removeFixed: true }
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

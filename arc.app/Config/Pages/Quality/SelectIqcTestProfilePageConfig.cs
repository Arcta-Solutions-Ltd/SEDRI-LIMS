using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class SelectIqcTestProfilePageConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'selectiqctestprofilepage',
                pageTitle: '@QuaAddB@',
                text: '@QuaAddC@.',
                required: 'TestProfileId',
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
                                    { id: 'TestProfileId', type: 'dropdown', label: '@QuaIqcTesProA@', required: true, placeholder: '@QuaIqcTesProA@', optionsName: 'IqcTestProfiles', dynamic: true },
                                ]
                            }
                        ]
                    }
                ]
            }";
        }
    }
}

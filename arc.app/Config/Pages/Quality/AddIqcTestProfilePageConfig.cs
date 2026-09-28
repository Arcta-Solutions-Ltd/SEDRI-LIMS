using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class AddIqcTestProfilePageConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                name: 'addiqctestprofilepage',
                pageTitle: '@QuaAddProA@',
                text: '@QuaAddProB@.',
                columns: [
                    { 
                        key: 'col1',
                        fieldWidth: 'wide',
                        itemWidth: 'wide',
                        formGroups: [
                            { 
                                key: 'fg1',
                                fields: [
                                    { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@QuaIqcTesProNam@' },
                                    { id: 'TestMethodId', type: 'dropdown', label: '@BreTes@', required: true, placeholder: '@BreTes@', optionsName: 'TestMethod', dynamic: true },
                                ]
                            }
                        ]
                    }
                ]
            }";
        }
    }
}

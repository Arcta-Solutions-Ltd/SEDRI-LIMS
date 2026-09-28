using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class EditIqcTestProfileDefaultPageConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                name: 'editiqctestprofiledefaultpageconfig',
                pageTitle: '@QuaDef@',
                text: '@QuaDo@.',
                columns: [
                    {
                        key: 'col1',
                        formGroups: [
                            {
                                key: 'fg1',
                                fields: [
                                    { id: 'Default', type: 'toggle', label: '@QuaUse@', required: true, default: 'No' }
                                ]
                            }
                        ]
                    }
                ]
            }";
        }
    }
}

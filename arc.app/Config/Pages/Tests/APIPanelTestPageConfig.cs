using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class APIPanelTestPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'apipaneltestpage',
                            pageTitle: '@TesApi@',
                            text: '@TesEntU@.',
                            configureActions: 'add,edit',
                            columns: [
                                { 
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        { 
                                            key: 'fg1',
                                            fields: [
                                                { id: 'APIIDPanel', type: 'combobox', label: '@SpeApi@', required: true, placeholder: '@SpeSelE@', optionsName: 'SpecimenApiIdPanel' },
                                                { id: 'IdProfile', type: 'singleline', label: '@SpeId@', required: true, placeholder: '@SpeSelF@' },
                                                { id: 'PercentageID', type: 'singleline', label: '@SpeIdA@', required: true, placeholder: '@SpeEntF@' },
                                                { id: 'printonreport', type: 'toggle', label: '@GenDis@', defaultValue: 'Yes', Configurable: 'No'}
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

            return page;
        }
    }
}

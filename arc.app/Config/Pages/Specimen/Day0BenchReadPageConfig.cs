using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class Day0BenchReadPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{
                            name: 'day0benchread',
                            pageTitle: '@SpePro@',
                            text: '@SpeAss@.',
                            nextItemButton: { buttontext: '@SpeNex@', show: true },
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'ReceivedConditionId', type: 'dropdown', label: '@SpeSpeE@', required: false, multiselect: false, placeholder: '@SpeSelA@', optionsName: 'ReceivedCondition', defaultValue: '28' },
                                                { id: 'SpecimenAppearanceId', type: 'combobox', label: '@SpeSpeF@', required: false, placeholder: '@SpeSelB@', optionsName: 'SpecimenAppearance', parentList: 'SpecimenTypeId', Configurable: 'No' },
                                                { id: 'SpecimenWeight', type: 'number', label: '@SpeSpeD@', required: false, placeholder: '@SpeEntB@', Min: '0', Max: '999', MaxDPs: '0' },
                                                { id: 'BenchReadDay0Action', type: 'radio', label: '@GenAct@', required: false, optionsName: 'BenchReadDay0Action', defaultValue: '591' }
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

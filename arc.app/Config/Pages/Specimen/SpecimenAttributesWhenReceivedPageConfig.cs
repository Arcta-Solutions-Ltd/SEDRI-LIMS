using arc.app.Common;

namespace arc.app.Config.Pages
{
    internal class SpecimenAttributesWhenReceivedPageConfig : IDefinition
    {
        public string Get()
        {
            var page = @"{ 
                            name: 'specimenattributeswhenreceived',
                            pageTitle: '@SpeSpeJ@',
                            text: '@SpeProD@.',
                            required: 'SpecimenTypeId, ReceivedConditionId',
                            requiredRule: 'and',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'SpecimenTypeId', type: 'combobox', label: '@SpeSpeB@', required: true, multiselect: false, placeholder: '@SpeSelC@', optionsName: 'SpecimenType' },
                                                { id: 'SpecimenSiteId', type: 'combobox', label: '@SpeSpeC@', multiselect: false, placeholder: '@SpeSelD@', optionsName: 'SpecimenSite', parentList: 'SpecimenTypeId' }
                                            ]
                                        }, 
                                        {
                                            key: 'fg2',
                                            fields: [
                                                { id: 'ReceivedConditionId', type: 'combobox', label: '@SpeSpeE@', required: true, multiselect: false, placeholder: '@SpeSelA@', optionsName: 'ReceivedCondition', defaultValue: '28' },
                                                { id: 'SpecimenAppearanceId', type: 'combobox', label: '@SpeSpeF@', required: false, placeholder: '@SpeSelB@', optionsName: 'SpecimenAppearance', parentList: 'SpecimenTypeId' },
                                                { id: 'Age', type: 'age', label: '@SpeAge@', required: false, Configurable: 'No' },
                                                { id: 'ExistingBarCode', type: 'singleline', label: '@SpeExi@', required: false, placeholder: '@SpeEntE@', Max: 30}
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

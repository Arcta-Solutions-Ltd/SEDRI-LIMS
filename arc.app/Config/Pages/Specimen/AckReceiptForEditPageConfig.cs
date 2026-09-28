using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Initial Assessment page for the edit specimen workflow.
/// Matches the add received specimen acknowledgment page without Immediate Action controls.
/// </summary>
internal class AckReceiptForEditPageConfig : IDefinition
{
    public string Get()
    {
        var page = @"{
                            name: 'ackreceiptpageforedit',
                            pageTitle: '@SpeAckB@',
                            text: '@SpeProA@.',
                            required: 'ReceivedDate,ReceivedConditionId',
                            requiredRule: 'and',
                            configureActions: 'edit',
                            columns: [
                                {
                                    key: 'col1',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'ReceivedDate', type: 'date', label: '@SpeRecD@', required: true, defaultToNow: true, placeholder: '@SpeSel@', Min: 'now d-300', Max: 'now', Configurable: 'No' },
                                                { id: 'ReceivedTime', type: 'time', label: '@SpeRecE@', required: true, defaultToNow: true, placeholder: '@SpeEntA@', Configurable: 'No' },
                                                { id: 'ReceivedConditionId', type: 'combobox', label: '@SpeSpeE@', required: true, multiselect: false, placeholder: '@SpeSelA@', optionsName: 'ReceivedCondition', defaultValue: '28', Configurable: 'No' },
                                                { id: 'SpecimenAppearanceId', type: 'combobox', label: '@SpeSpeF@', required: false, placeholder: '@SpeSelB@', optionsName: 'SpecimenAppearance', parentList: 'SpecimenTypeId', Configurable: 'No' }
                                            ]
                                        },
                                        {
                                            key: 'fg2',
                                            rules:[{ effect: 'visible', field: 'SpecimenTypeId', rule: '=', value: '808'}],
                                            fields: [
                                                { id: 'BottleOnlyWeight', type: 'number', label: '@SpeBot@', required: false, placeholder: '@SpeEntB@', Min: '0', Max: '99', MaxDPs: '2', Suffix: 'g', Configurable: 'No'},
                                                { id: 'BloodAndBottleWeight', type: 'number', label: '@SpeBlo@', required: false, placeholder: '@SpeEntB@', Min: '0', Max: '99', MaxDPs: '2', Suffix: 'g', Configurable: 'No'}
                                            ]
                                        },
                                        {
                                            key: 'fg4',
                                            fields: [
                                                { id: 'ManufacturersBarcode', type: 'singleline', label: '@InsManD@', required: false, placeholder: '@InsSca@', Configurable: 'No', Max: 30 },
                                                { id: 'TestCategoryId', type: 'combobox', label: '@GenTesG@', required: false, placeholder: '@GenSelQ@', optionsName: 'TestCategory', multiSelect: true },
                                                { id: 'CultureTypeCategoryId', type: 'combobox', label: '@GenCul@', required: false, placeholder: '@GenSelQ@', optionsName: 'CultureTypeCategory', multiSelect: true }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}

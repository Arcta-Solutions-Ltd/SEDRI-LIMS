using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Provides the JSON definition for the acknowledgment receipt page configuration.
/// Implements <see cref="IDefinition"/> to supply the page metadata, layout, and behavior rules.
/// </summary>
internal class ACKReceiptPageConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON string that defines the acknowledgment receipt page settings.
    /// </summary>
    /// <returns>
    /// A JSON-formatted string containing:
    /// - name, pageTitle, and descriptive text
    /// - required fields and validation rules
    /// - form grouping, field definitions, and conditional visibility
    /// - column layout and nextButton state transitions
    /// </returns>
    public string Get()
    {
        var page = @"{
                            name: 'ackreceiptpage',
                            pageTitle: '@SpeAckB@',
                            text: '@SpeProA@.',
                            required: 'ReceivedDate,ReceivedConditionId',
                            requiredRule: 'and',
                            configureActions: 'add,edit',
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
                                        },
                                        {
                                            key: 'fg5',
                                            fields: [
                                                { id: 'Action', type: 'radio', label: '@SpeImm@', required: false, optionsName: 'immediateAction', defaultValue: '510', Configurable: 'No' }
                                            ]
                                        },
                                        {
                                            key: 'fg6',
                                            rules:[{ effect: 'visible', field: 'Action', rule: '=', value: '507'}],
                                            fields: [
                                                { id: 'SelectReasonId', type: 'combobox', label: '@SpeReaG@', required: true, multiselect: false, placeholder: '@SpeEntC@', optionsName: 'RejectionReasonList' },
                                                { id: 'RejectionReason', type: 'multiline', label: '@SpeReaH@', required: false, placeholder: '@SpeEntC@' }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: {
                                onclickstate: {
                                    state: 'notrejected',
                                    rules:[
                                        { effect: 'notrejected', field: 'Action', rule: '!=', value: '507' },
                                        { effect: 'rejected', field: 'Action', rule: '=', value: '507' }
                                    ]
                                },
                                buttontext: '@GenNex@',
                                show: true
                            },
                        }";

        return page;
    }
}



//nextButton:
//{
//enabledState: 'validspecimen',
//                                          onclickstate: { state: 'rejectspecimen', rules: [{ effect: 'rejectspecimen', field: 'Action', rule: '=', value: '507'}] },
//                                          buttontext: '@GenNex@',
//                                          show: true
//                                        },


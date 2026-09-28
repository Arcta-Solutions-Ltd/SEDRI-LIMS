using arc.app.Common;

namespace arc.app.Config.Pages.Billing;

internal class EditBillingRecordPageConfig : IDefinition
{
    public string Get()
    {
        var page = @"{
                            name: 'editbillingrecordpage',
                            pageTitle: '@GenEdi@',
                            text: '@BilRecEdiH@',
                            columns: [
                                {
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'PatientId', type: 'text', label: '@BilPat@', Configurable: 'No' },
                                                { id: 'SpecimenId', type: 'text', label: '@BilSpe@', Configurable: 'No' },
                                                { id: 'CultureId', type: 'text', label: '@BilCul@', Configurable: 'No' },
                                                { id: 'DirectTestName', type: 'text', label: '@BilDir@', Configurable: 'No' },
                                                { id: 'TestPatternId', type: 'text', label: '@BilTes@', Configurable: 'No' },
                                                { id: 'Description', type: 'multiline', label: '@GenDes@', required: false, Max: 2000 },
                                                { id: 'TotalAmount', type: 'number', label: '@BilAmt@', required: true, Min: '0', MaxDPs: '2' }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}

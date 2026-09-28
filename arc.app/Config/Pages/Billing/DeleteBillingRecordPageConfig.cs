using arc.app.Common;

namespace arc.app.Config.Pages.Billing;

internal class DeleteBillingRecordPageConfig : IDefinition
{
    public string Get()
    {
        var page = @"{
                            name: 'deletebillingrecordpage',
                            pageTitle: '@GenDel@',
                            text: '@BilRecDelH@',
                            required: 'DirectTestName',
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
                                                { id: 'DirectTestName', type: 'text', label: '@BilDir@', placeholder: '@GenLisA@' }
                                            ]
                                        }
                                    ]
                                }
                            ],
                            nextButton: { show: true, buttonText: '@GenDelC@' }
                        }";

        return page;
    }
}

using arc.app.Common;

namespace arc.app.Config.Pages.Billing;

internal class DeleteBillingRulePageConfig : IDefinition
{
    public string Get()
    {
        var page = @"{
                            name: 'deletebillingrulepage',
                            pageTitle: '@GenDel@',
                            text: '@BilRulDel@',
                            required: 'Name',
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
                                                { id: 'Name', type: 'text', label: '@GenNam@', placeholder: '@GenLisA@' }
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

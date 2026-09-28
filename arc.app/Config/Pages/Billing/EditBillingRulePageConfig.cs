using arc.app.Common;

namespace arc.app.Config.Pages.Billing;

internal class EditBillingRulePageConfig : IDefinition
{
    public string Get()
    {
        var page = @"{
                            name: 'editbillingrulepage',
                            pageTitle: '@GenEdi@',
                            text: '@BilRulEdi@',
                            columns: [
                                {
                                    key: 'col1',
                                    fieldWidth: 'wide',
                                    itemWidth: 'wide',
                                    formGroups: [
                                        {
                                            key: 'fg1',
                                            fields: [
                                                { id: 'Name', type: 'singleline', label: '@GenNam@', required: true, placeholder: '@BilRulNam@', Max: 200 }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }";

        return page;
    }
}

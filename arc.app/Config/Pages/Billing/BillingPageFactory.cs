using arc.app.Common;

namespace arc.app.Config.Pages.Billing;

internal class BillingPageFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addbillingrulepage" => new AddBillingRulePageConfig(),
            "editbillingrulepage" => new EditBillingRulePageConfig(),
            "deletebillingrulepage" => new DeleteBillingRulePageConfig(),
            "editbillingrecordpage" => new EditBillingRecordPageConfig(),
            "deletebillingrecordpage" => new DeleteBillingRecordPageConfig(),
            _ => null,
        };
    }
}

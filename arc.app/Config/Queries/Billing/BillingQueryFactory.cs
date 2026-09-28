using arc.app.Common;

namespace arc.app.Config.Queries.Billing;

internal class BillingQueryFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "billingrulelist" => new BillingRuleListQuery(),
            "singlebillingruleforlist" => new SingleBillingRuleForListQuery(),
            "billingrecordlist" => new BillingRecordListQuery(),
            "singlebillingrecordforlist" => new SingleBillingRecordForListQuery(),
            _ => null,
        };
    }
}

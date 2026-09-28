using arc.app.Common;

namespace arc.app.Config.Queries.Billing;

internal class SingleBillingRuleForListQuery : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'SingleBillingRuleForList',
            'TableName': 'BillingRule',
            'Type': 'Single',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int' },
                { 'Name': 'Name', 'Type': 'string' },
                { 'Name': 'LaboratoryId', 'Type': 'int' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': 'equals' },
                { 'Field': 'LaboratoryId', 'Comparison': 'equals' }
            ]
        }";
    }
}

using arc.app.Common;

namespace arc.app.Config.Queries.Billing;

internal class BillingRuleListQuery : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'BillingRuleList',
            'TableName': 'BillingRule',
            'Type': 'Select',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int' },
                { 'Name': 'Name', 'Type': 'string' },
                { 'Name': 'LaboratoryId', 'Type': 'int' }
            ],
            'Where': [
                { 'Field': 'LaboratoryId', 'Comparison': 'equals' },
                { 'Field': 'Name', 'Comparison': 'contains', 'orGroup': 'search' }
            ],
            'OrderBy': 'Name',
            'Descending': false
        }";
    }
}

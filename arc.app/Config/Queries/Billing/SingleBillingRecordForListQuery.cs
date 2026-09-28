using arc.app.Common;

namespace arc.app.Config.Queries.Billing;

internal class SingleBillingRecordForListQuery : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'SingleBillingRecordForList',
            'TableName': 'BillingRecord',
            'Type': 'Single',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int' },
                { 'Name': 'PatientId', 'Type': 'int' },
                { 'Name': 'SpecimenId', 'Type': 'int' },
                { 'Name': 'CultureId', 'Type': 'int' },
                { 'Name': 'DirectTestName', 'Type': 'string' },
                { 'Name': 'TestPatternId', 'Type': 'int' },
                { 'Name': 'Description', 'Type': 'string' },
                { 'Name': 'TotalAmount', 'Type': 'numeric' },
                { 'Name': 'LaboratoryId', 'Type': 'int' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': 'equals' },
                { 'Field': 'LaboratoryId', 'Comparison': 'equals' }
            ]
        }";
    }
}

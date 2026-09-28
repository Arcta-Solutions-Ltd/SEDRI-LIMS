using arc.app.Common;

namespace arc.app.Config.Queries.Billing;

internal class BillingRecordListQuery : IDefinition
{
    public string Get()
    {
        return @"{
            'Query': 'BillingRecordList',
            'TableName': 'BillingRecord',
            'Type': 'Select',
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
                { 'Field': 'LaboratoryId', 'Comparison': 'equals' },
                { 'Field': 'Description', 'Comparison': 'contains', 'orGroup': 'search' },
                { 'Field': 'DirectTestName', 'Comparison': 'contains', 'orGroup': 'search' }
            ],
            'OrderBy': 'Id',
            'Descending': true
        }";
    }
}

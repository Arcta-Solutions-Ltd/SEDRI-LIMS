using arc.app.Common;

namespace arc.app.Config.Queries.Export;

/// <summary>
/// Count query used by add export schedule DataRules to reject duplicate schedule names per profile.
/// </summary>
internal class ExportScheduleAlreadyExistsForAddQuery : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{
            'Query': 'exportschedulealreadyexistsforadd',
            'TableName': 'exportschedule',
            'Type': 'count',
            'Fields': [
                { 'Name': 'id', 'Type': 'string' }
            ],
            'Where': [
                { 'Field': 'name', 'Comparison': 'equals', 'FieldToMatch': 'name' },
                { 'Field': 'exportprofileid', 'Comparison': 'equals', 'FieldToMatch': 'exportprofileid' }
            ]
        }";
    }
}

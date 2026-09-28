using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Query definition for loading alert approval history for a given alert.
/// Used by the alert approvals list view section on the alert record view.
/// </summary>
internal class AlertApprovalListByAlertIdQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the alert approval list query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'AlertApprovalListByAlertId',
            'TableName': 'alertapproval',
            'Type': 'Select',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int' },
                { 'Name': 'DateRecorded', 'Type': 'datetime' },
                { 'Name': 'RecordedBy', 'Type': 'string' },
                { 'Name': 'CodingStatusId', 'Type': 'int' }
            ],
            'Joins': [
                { 'Table': 'ListItem', 'Type': 'Left', 'On': 'CodingStatusId', 'From': 'Id', 'Fields': [{'Name': 'Value', 'KnownAs': 'CodingStatus'}] }
            ],
            'Where': [
                { 'Field': 'AlertId', 'Comparison': '=' }
            ],
            'Orderby': 'Id'
        }";
    }
}

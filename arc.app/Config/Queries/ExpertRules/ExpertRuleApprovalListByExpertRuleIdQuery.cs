using arc.app.Common;

namespace arc.app.Config.Queries.ExpertRules;

/// <summary>
/// Query definition for loading expert rule approval history for a given expert rule.
/// Used by the expert rule approvals list view section on the expert rule record view.
/// </summary>
internal class ExpertRuleApprovalListByExpertRuleIdQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the expert rule approval list query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'ExpertRuleApprovalListByExpertRuleId',
            'TableName': 'expertruleapproval',
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
                { 'Field': 'ExpertRuleId', 'Comparison': '=' }
            ],
            'Orderby': 'Id'
        }";
    }
}

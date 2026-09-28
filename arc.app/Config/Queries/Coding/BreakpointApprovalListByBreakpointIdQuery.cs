using arc.app.Common;

namespace arc.app.Config.Queries.Coding;

/// <summary>
/// Query definition for loading breakpoint approval history for a given breakpoint.
/// Used by the breakpoint approvals list view section on the breakpoint record view.
/// </summary>
internal class BreakpointApprovalListByBreakpointIdQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the breakpoint approval list query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'BreakpointApprovalListByBreakpointId',
            'TableName': 'breakpointapproval',
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
                { 'Field': 'BreakpointId', 'Comparison': '=' }
            ],
            'Orderby': 'Id'
        }";
    }
}

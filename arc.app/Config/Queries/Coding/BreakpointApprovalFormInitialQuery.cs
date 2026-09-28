using arc.app.Common;

namespace arc.app.Config.Queries.Coding;

/// <summary>
/// Minimal query that returns BreakpointId for the add breakpoint approval form.
/// Used to populate the form with the parent BreakpointId when opened from the embedded list Add button.
/// </summary>
internal class BreakpointApprovalFormInitialQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the breakpoint approval form initial query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'BreakpointApprovalFormInitialQuery',
            'TableName': 'Breakpoint',
            'Type': 'Single',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int', 'KnownAs': 'BreakpointId' }
            ],
            'Where': [
                { 'Field': 'Id', 'Comparison': '=' }
            ]
        }";
    }
}

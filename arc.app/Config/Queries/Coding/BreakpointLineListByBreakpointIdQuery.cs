using arc.app.Common;

namespace arc.app.Config.Queries.Coding;

/// <summary>
/// Query definition for loading breakpoint lines (resultline rows) for a given breakpoint.
/// Used by the breakpoint lines list view section on the breakpoint record view.
/// </summary>
internal class BreakpointLineListByBreakpointIdQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the breakpoint line list query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'BreakpointLineListByBreakpointId',
            'TableName': 'resultline',
            'Type': 'Special',
            'Fields': [
                { 'Name': 'Id', 'Type': 'int' },
                { 'Name': 'StartVal', 'Type': 'decimal' },
                { 'Name': 'EndVal', 'Type': 'decimal' }
            ],
            'Where': [
                { 'Field': 'BreakpointId', 'Comparison': '=' }
            ],
            'Orderby': 'Id'
        }";
    }
}

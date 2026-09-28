using arc.app.Common;

namespace arc.app.Config.Queries.Coding;

/// <summary>
/// Query definition for loading a single breakpoint for the breakpoint list (e.g. row selection or record view).
/// Resolved by a special handler; returns one breakpoint record for the given filter (e.g. Id).
/// </summary>
internal class SingleBreakpointForBreakpointListQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the single-breakpoint query: table Breakpoint, type Special.
    /// </summary>
    /// <returns>JSON string defining the query name, table, and type.</returns>
    public string Get()
    {
        return @"{
                        'Query': 'SingleBreakpointForBreakpointListQuery',
                        'TableName': 'Breakpoint',
                        'Type': 'Special',
                    }";
    }
}



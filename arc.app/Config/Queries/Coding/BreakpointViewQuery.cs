using arc.app.Common;

namespace arc.app.Config.Queries;

/// <summary>
/// Definition for the breakpoint view query. Supplies the query configuration used when loading
/// a single breakpoint for view (e.g. on a breakpoint record/detail screen).
/// </summary>
/// <remarks>
/// Returns a special query config that uses BreakpointViewQuery for execution and
/// breakpointviewmapper for result mapping. Translate is enabled for localized display.
/// </remarks>
internal class BreakpointViewQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the breakpoint view query.
    /// </summary>
    /// <returns>Query definition with Query name, TableName, Type, ResultMapping, and Translate flag.</returns>
    public string Get()
    {
        return @"{ 
                    'Query': 'BreakpointViewQuery',
                    'TableName': 'Breakpoint',
                    'Type': 'Special',
                    'ResultMapping': 'breakpointviewmapper',
                    'Translate': true
                }";
    }
}



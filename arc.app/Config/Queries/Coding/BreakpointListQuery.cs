using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Definition for the breakpoint list query. Supplies the query configuration used when loading
    /// the list of breakpoints (e.g. on the breakpoint list/search screen).
    /// </summary>
    /// <remarks>
    /// Uses type 'Special' and is executed by the BreakpointList query runner in the app layer.
    /// </remarks>
    internal class BreakpointListQuery : IDefinition
    {
        /// <summary>
        /// Returns the JSON configuration for the breakpoint list query.
        /// </summary>
        /// <returns>Query definition with Query name 'BreakpointList', TableName 'Breakpoint', and Type 'Special'.</returns>
        public string Get()
        {
            return @"{ 
                        'Query': 'BreakpointList',
                        'TableName': 'Breakpoint',
                        'Type': 'Special'
            }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Configuration class for the breakpoint by ID for edit query.
    /// </summary>
    internal class BreakpointByIdForEditQuery : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the breakpoint by ID for edit query.
        /// </summary>
        /// <returns>A JSON string that represents the query configuration.</returns>
        public string Get()
        {
            return @"{ 
                    'Query': 'BreakpointByIdForEdit',
                    'TableName': 'Breakpoint',
                    'Type': 'Single',
                    'ResultMapping': 'breakpointbyidforeditresultmapping',
                    'Fields': [
                        {'Name': 'Id', 'Type': 'string' }
                    ],
                    'Joins': [
                        { 'Table': 'Antibiotic', 'Fields': [{'Name': 'AntibioticName'}] }
                    ],
                    'ListItems': 'TestMethod,Host',
                    'Where' : [
                        {'Field': 'Id', 'Comparison': '=' }
                    ],
                    'Orderby': 'AntibioticName'
                }";
        }
    }
}

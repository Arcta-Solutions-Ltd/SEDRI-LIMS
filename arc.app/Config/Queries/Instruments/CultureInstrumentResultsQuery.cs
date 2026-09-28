using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Configuration class for the culture instrument results query.
    /// </summary>
    internal class CultureInstrumentResultsQuery : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the culture instrument results query.
        /// </summary>
        /// <returns>A JSON string that represents the query configuration.</returns>
        public string Get()
        {
            return @"{ 
                    'Query': 'CultureInstrumentResults',
                    'TableName': 'InstrumentResults',
                    'Type': 'Special'
                }";
        }
    }
}

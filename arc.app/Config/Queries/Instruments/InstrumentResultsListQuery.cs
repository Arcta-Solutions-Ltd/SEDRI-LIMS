using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Represents the definition for an instrument results list query.
    /// </summary>
    internal class InstrumentResultsListQuery : IDefinition
    {
        /// <summary>
        /// Gets the query definition as a JSON string.
        /// </summary>
        /// <returns>The query definition in JSON format.</returns>
        public string Get()
        {
            return @"{ 
                        'Query': 'InstrumentResultsListQuery',
                        'TableName': 'InstrumentResults',
                        'Translate': false,
                        'Type': 'special'
                    }";
        }
    }
}
 


using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Configuration class for the instrument request query.
    /// </summary>
    internal class InstrumentRequestQuery : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the instrument request query.
        /// </summary>
        /// <returns>A JSON string that represents the query configuration.</returns>
        public string Get()
        {
            return @"{ 
                    'Query': 'InstrumentRequestQuery',
                    'TableName': 'InstrumentResults',
                    'Translate': false,
                    'Type': 'special'
                }";
        }
    }
}

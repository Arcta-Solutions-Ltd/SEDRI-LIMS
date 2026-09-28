using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Configuration class for the instrument profile list query.
    /// </summary>
    internal class InstrumentProfileListQuery : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the instrument profile list query.
        /// </summary>
        /// <returns>A JSON string that represents the query configuration.</returns>
        public string Get()
        {
            return @"{ 
                    'Query': 'InstrumentProfileListQuery',
                    'TableName': 'Configs',
                    'Translate': true,
                    'Type': 'special'
                }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Configuration class for the edit instrument profile query.
    /// </summary>
    internal class EditInstrumentProfileQuery : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the edit instrument profile query.
        /// </summary>
        /// <returns>A JSON string that represents the query configuration.</returns>
        public string Get()
        {
            return @"{ 
                    'Query': 'EditInstrumentProfileQuery',
                    'TableName': 'Config',
                    'Translate': true,
                    'Type': 'special'
                }";
        }
    }
}

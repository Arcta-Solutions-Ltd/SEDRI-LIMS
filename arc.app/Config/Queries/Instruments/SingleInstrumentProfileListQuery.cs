using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Configuration class for the single instrument profile list query.
    /// </summary>
    internal class SingleInstrumentProfileListQuery : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the single instrument profile list query.
        /// </summary>
        /// <returns>A JSON string that represents the query configuration.</returns>
        public string Get()
        {
            return @"{ 
                    'Query': 'SingleInstrumentProfileListQuery', 
                    'TableName': 'Configs',                      
                    'Translate': true,                           
                    'Type': 'special'                            
                }";
        }
    }

}

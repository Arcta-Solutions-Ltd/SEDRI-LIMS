using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Configuration class for the instrument error JSON contents query.
    /// </summary>
    internal class InstrumentErrorJsonContentsQuery : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the instrument error JSON contents query.
        /// </summary>
        /// <returns>A JSON string that represents the query configuration.</returns>
        public string Get()
        {
            return @"{ 
                    'Query': 'InstrumentErrorJsonContentsQuery', 
                    'TableName': 'InstrumentErrors', 
                    'Type': 'Single', 
                    'Fields': [
                        {'Name': 'Id', 'Type': 'string'},
                        {'Name': 'Message', 'Type': 'string'}
                    ],
                    'Where' : [
                        {'Field': 'Id', 'Comparison': '=' } 
                    ]
                }";
        }
    }
}

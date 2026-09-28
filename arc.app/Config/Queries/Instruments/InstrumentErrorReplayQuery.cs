using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Configuration class for the instrument error replay query.
    /// </summary>
    internal class InstrumentErrorReplayQuery : IDefinition
    {
        /// <summary>
        /// Generates the JSON string for the instrument error replay query.
        /// </summary>
        /// <returns>A JSON string that represents the query configuration.</returns>
        public string Get()
        {
            return @"{ 
                    'Query': 'InstrumentErrorReplayQuery',
                    'TableName': 'InstrumentErrors',
                    'Type': 'Single',
                    'Fields': [
                        {'Name': 'Id', 'Type': 'string' },
                        {'Name': 'ErrorText', 'Type': 'string' },
                        {'Name': 'Message', 'Type': 'string' }
                    ],
                    'Where' : [
                        {'Field': 'Id', 'Comparison': '=' }
                    ],
                    'Translate': true
                }";
        }
    }
}

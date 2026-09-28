using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// This class represents the definition of the instrument error list query.
    /// </summary>
    internal class InstrumentErrorListQuery : IDefinition
    {
        /// <summary>
        /// Retrieves the query definition for the instrument error list.
        /// </summary>
        /// <returns>A string containing the query definition in JSON format.</returns>
        public string Get()
        {
            return @"{
                    'Query': 'InstrumentErrorListQuery',
                    'TableName': 'InstrumentErrors',
                    'Type': 'Select',
                    'Translate': true,
                    'Fields': [
                        { 'Name': 'Id', 'Type': 'int' },
                        { 'Name': 'ProfileName', 'Type': 'string' },
                        { 'Name': 'ErrorText', 'Type': 'string' },
                        { 'Name': 'LastModifiedDate', 'Type': 'datetime' },
                        { 'Name': 'Message', 'Type': 'string', 'KnownAs': 'TestResults' }
                    ],
                    'ListItems': 'InstrumentDirection,ErrorStatus',
                    'Where': [
                        { 'Field': 'InstrumentDirectionId', 'Comparison': 'oneof' },
                        { 'Field': 'InstrumentResultId', 'Comparison': 'oneof' },
                        { 'Field': 'ProfileName', 'Comparison': 'contains', 'orGroup': 'search' },
                        { 'Field': 'ErrorText', 'Comparison': 'contains', 'orGroup': 'search' }
                    ],
                    'Orderby': 'lastmodifieddate'
                }";
        }
    }
}

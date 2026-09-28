using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Query configuration for GetOrCreateCultureTestId. Returns the culturetest ID for a given culture and test name,
    /// creating the record if it does not exist.
    /// </summary>
    internal class GetOrCreateCultureTestIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'getorcreateculturetestid', 'TableName': 'CultureTests', 'Type': 'Special'
                     }";
        }
    }
}

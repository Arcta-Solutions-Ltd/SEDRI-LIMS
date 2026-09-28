using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class RemoveCultureTestQueryConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'RemoveCultureTestQuery', 'TableName': 'culturetests', 'Type': 'special', 'Translate': true}";
        }
    }
}

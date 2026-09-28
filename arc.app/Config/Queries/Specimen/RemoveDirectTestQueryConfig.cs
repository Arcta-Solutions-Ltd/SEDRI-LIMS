using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class RemoveDirectTestQueryConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'RemoveDirectTestQuery', 'TableName': 'tests', 'Type': 'special', 'Translate': true}";
        }
    }
}

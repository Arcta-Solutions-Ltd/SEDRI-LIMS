using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class CultureTestConfigListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'CultureTestConfigListQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}

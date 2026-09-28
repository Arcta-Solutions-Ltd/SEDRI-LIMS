using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class DirectTestConfigListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'DirectTestConfigListQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}

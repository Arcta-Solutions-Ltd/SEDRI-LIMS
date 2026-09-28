using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class MenuItemConfigListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'MenuItemConfigListQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}

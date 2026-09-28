using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class PagesInFormQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'PagesInFormQuery', 'Type': 'Config', 'translate': true}";
        }
    }
}

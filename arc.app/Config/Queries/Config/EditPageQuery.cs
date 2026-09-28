using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditPageQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'EditPageQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class DeleteSectionQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'DeleteSectionQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}

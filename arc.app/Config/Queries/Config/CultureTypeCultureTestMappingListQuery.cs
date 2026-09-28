using arc.app.Common;

namespace arc.app.Config.Queries
{ 
    internal class CultureTypeCultureTestMappingListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'CultureTypeCultureTestMappingListQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}

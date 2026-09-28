using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SpecimenTypeCultureTypeMappingListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'SpecimenTypeCultureTypeMappingListQuery', 'Type': 'Config'}";
        }
    }
}

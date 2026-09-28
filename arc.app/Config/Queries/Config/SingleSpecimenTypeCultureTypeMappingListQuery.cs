using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SingleSpecimenTypeCultureTypeMappingListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'SingleSpecimenTypeCultureTypeMappingListQuery', 'Type': 'Config'}";
        }
    }
}

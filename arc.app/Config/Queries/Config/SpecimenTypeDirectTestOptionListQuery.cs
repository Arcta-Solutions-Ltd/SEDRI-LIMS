using arc.app.Common;

namespace arc.app.Config.Queries.Config;
internal class SpecimenTypeDirectTestOptionListQuery : IDefinition
{
    public string Get()
    {
        return @"{ 'Query': 'SpecimenTypeDirectTestOptionListQuery', 'Type': 'Config', 'Translate': true}";
    }
}

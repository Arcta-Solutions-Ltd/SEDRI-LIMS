using arc.app.Common;

namespace arc.app.Config.Queries.Config;
internal class DeleteMappingQuery : IDefinition
{
    public string Get()
    {
        return @"{ 'Query': 'deletemappingquery', 'Type': 'Config', 'Translate': true}";

    }
}

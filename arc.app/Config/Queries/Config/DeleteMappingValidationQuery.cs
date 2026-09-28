using arc.app.Common;

namespace arc.app.Config.Queries.Config;
internal class DeleteMappingValidationQuery : IDefinition
{
    public string Get()
    {
        return @"{ 'Query': 'deletemappingvalidationquery', 'Type': 'Config', 'Translate': true}";

    }
}

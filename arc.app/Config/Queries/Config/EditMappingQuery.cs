using arc.app.Common;

namespace arc.app.Config.Queries.Config;
internal class EditMappingQuery : IDefinition
{
    public string Get()
    {
        return @"{ 'Query': 'editmappingquery', 'Type': 'Config', 'Translate': true}";
    }
}

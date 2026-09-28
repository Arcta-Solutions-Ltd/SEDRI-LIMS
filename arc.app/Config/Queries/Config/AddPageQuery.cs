using arc.app.Common;

namespace arc.app.Config.Queries;

internal class AddPageQuery : IDefinition
{
    public string Get()
    {
        return @"{ 'Query': 'AddPageQuery', 'Type': 'Config', 'Translate': true}";
    }
}

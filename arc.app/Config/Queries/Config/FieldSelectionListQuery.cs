using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class FieldSelectionListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'FieldSelectionListQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditSectionQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'EditSectionQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}

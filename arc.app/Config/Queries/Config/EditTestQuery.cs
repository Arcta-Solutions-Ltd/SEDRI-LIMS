using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditTestQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'EditTestQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditFieldQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'EditFieldQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}

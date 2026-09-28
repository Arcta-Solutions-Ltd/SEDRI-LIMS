using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class FieldsForEventQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'FieldsForEventQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}

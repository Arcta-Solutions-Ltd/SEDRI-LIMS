using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class FormConfigListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'FormConfigListQuery', 'Type': 'Config', 'translate': true}";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ReportDefinitionQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'ReportDefinitionQuery', 'Type': 'Config', 'translate': true}";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ReportConfigListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'ReportConfigListQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}

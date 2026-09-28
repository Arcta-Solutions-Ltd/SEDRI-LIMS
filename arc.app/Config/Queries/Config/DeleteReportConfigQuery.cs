using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class DeleteReportConfigQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'DeleteReportConfigQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}

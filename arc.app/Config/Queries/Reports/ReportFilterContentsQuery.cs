using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ReportFilterContentsQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'reportfiltercontentsquery', 'Type': 'Report', 'Translate': true}";
        }
    }
}

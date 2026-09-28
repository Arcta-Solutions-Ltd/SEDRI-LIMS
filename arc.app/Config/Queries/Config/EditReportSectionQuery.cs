using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditReportSectionQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'EdiReportSectionQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}

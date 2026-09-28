using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditReportConfigQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'EditReportConfigQuery', 'Type': 'Config', 'Translate': true}";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SpecimenRecordReportQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 'Query': 'specimenrecordreport', 'Type': 'Report', 'Translate': true}";
        }
    }
}

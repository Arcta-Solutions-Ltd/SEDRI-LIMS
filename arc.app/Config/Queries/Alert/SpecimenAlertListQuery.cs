using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SpecimenAlertListQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                'Query': 'specimenalertlist', 'TableName': 'Alert', 'Type': 'Special'
            }";
        }
    }
}

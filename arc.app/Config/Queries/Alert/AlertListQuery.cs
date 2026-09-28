using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class AlertListQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                'Query': 'alertlist', 'TableName': 'Alert', 'Type': 'Special'
            }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SingleAlertForAlertListQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                'Query': 'SingleAlertForAlertList', 'TableName': 'Alert', 'Type': 'Special'
            }";
        }
    }
}

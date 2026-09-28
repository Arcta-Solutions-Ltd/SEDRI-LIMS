using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class EditAlertQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                'Query': 'editalert', 'TableName': 'Alert', 'Type': 'Special'
            }";
        }
    }
}

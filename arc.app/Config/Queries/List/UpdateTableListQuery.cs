using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class UpdateTableListQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                        'Query': 'updatetablelistquery', 'TableName': 'ListItem', 'Type': 'Special'
                    }";
        }
    }
}

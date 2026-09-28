using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ListContentsQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                        'Query': 'listcontents', 'TableName': 'ListItem', 'Type': 'Special'
                    }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SingleTableEntryForListQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'singletableentryforlist', 'TableName': 'ListItem', 'Type': 'Special'
                    }";
        }
    }
}

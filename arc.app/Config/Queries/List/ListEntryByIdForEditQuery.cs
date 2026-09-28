using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ListEntryByIdForEditQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'listentrybyid', 'TableName': 'List', 'Type': 'Special'
                    }";
        }
    }
}

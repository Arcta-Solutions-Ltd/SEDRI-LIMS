using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class ListByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'listbyid', 'TableName': 'List', 'Type': 'Special'
                    }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class UserByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'userbyid', 'TableName': 'Users', 'Type': 'special'
                    }";
        }
    }
}

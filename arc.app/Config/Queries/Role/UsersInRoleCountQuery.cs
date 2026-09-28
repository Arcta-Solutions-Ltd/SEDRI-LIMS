using arc.app.Common;

namespace arc.app.Config.Queries.Role
{
    internal class UsersInRoleCountQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'usersinrolecountquery', 'TableName': 'UserRole', 'Type': 'Count',
                        'ParameterMapping': 'usersinrolemapper',
                         'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'RoleId', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SingleRoleForRoleListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SingleRoleForRoleList',
                        'TableName': 'Role',
                        'Type': 'Single',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'},
                            {'Name': 'RoleName', 'Type': 'string'},
                            {'Name': 'Enabled', 'Type': 'string'},
                            {'Name': 'RoleDescription', 'Type': 'string'}
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ]
                    }";
        }
    }
}

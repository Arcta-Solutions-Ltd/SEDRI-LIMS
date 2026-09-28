using arc.app.Common;

namespace arc.app.Config.Queries.Role
{
    internal class RoleByIdForEditRoleQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'RoleByIdForEditRole', 'TableName': 'Role', 'Type': 'Single', 
                        'Fields': [
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

using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class RoleByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'RoleById', 'TableName': 'Role', 'Type': 'Single', 
                        'Fields': [
                            {'Name': 'RoleName', 'Type': 'string'},
                            {'Name': 'Enabled', 'Type': 'string'},
                            {'Name': 'RoleDescription', 'Type': 'string'},
                            {'Name': 'MenuPermission', 'Type': 'string' },
                            {'Name': 'EventPermission', 'Type': 'string' }
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ]
                    }";
        }
    }
}

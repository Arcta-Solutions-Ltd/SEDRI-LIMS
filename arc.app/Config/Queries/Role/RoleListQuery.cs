using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class RoleListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'RoleList', 'TableName': 'Role', 'Type': 'Select', 
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'},
                            {'Name': 'RoleName', 'Type': 'string'},
                            {'Name': 'Enabled', 'Type': 'string'},
                            {'Name': 'RoleDescription', 'Type': 'string'}
                        ],
                        'Orderby': 'RoleDescription',
                        'Where' : [
                            {'Field': 'RoleName', 'Comparison': 'contains', orGroup: 'search' },
                            {'Field': 'RoleDescription', 'Comparison': 'contains', orGroup: 'search' }
                        ]
                    }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Queries
{ 
    internal class RoleNameExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Query': 'rolenameexists', 'TableName': 'Role', 'Type': 'Count',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'RoleName', 'Comparison': 'equals' }
                        ]
                    }";
        }
    }
}

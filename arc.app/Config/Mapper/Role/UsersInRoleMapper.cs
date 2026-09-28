using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class UsersInRoleMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'usersinrolemapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target' : { 
                            'Name': 'usersinrolemapper', 
                            'Parameters': [ 
                                {'Key': 'RoleId', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class EditTableMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'edittablemapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Description', Value: 'Description' }
                        ],
                        'Target' : { 
                            Id: '<:1:>',
                            Description: '<:2:>'
                        }
                     }";
        }
    }
}

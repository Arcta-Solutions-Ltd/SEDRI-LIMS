using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class EditStateMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'editstatemapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'StateId', Value: 'StateId' }
                        ],
                        'Target': { 
                            ListId: 60,
                            Id: '<:2:>',
                            Value: '<:1:>'
                        }
                     }";
        }
    }
}

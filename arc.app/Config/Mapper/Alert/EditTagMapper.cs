using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class EditTagMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'edittagmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'TagId', Value: 'TagId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'ParentTagId', Value: 'ParentTagId' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'Enabled', Value: 'Enabled' }
                        ],
                        'Target': { 
                            ListId: 105,
                            Id: '<:2:>',
                            Value: '<:1:>',
                            ParentTagId: '<:3:>',
                            Enabled: '<:4:>'
                        }
                     }";
        }
    }
}

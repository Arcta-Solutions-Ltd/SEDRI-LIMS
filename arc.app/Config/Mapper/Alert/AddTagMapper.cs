using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AddTagMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'addtagmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'ParentTagId', Value: 'ParentTagId' }
                        ],
                        'Target' : { 
                            ListId: 105,
                            Value: '<:1:>',
                            ParentTagId: '<:2:>',
                            Fixed: false,
                            Enabled: true,
                            Deleted: false
                        }
                     }";
        }
    }
}

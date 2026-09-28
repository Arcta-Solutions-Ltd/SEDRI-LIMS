using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AddStateMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'addstatemapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            ListId: 60,
                            Value: '<:1:>',
                            ParentId: 524,
                            Enabled: true,
                            Fixed: false,
                            DisplayOrder: 1,
                            Deleted: false
                        }
                     }";
        }
    }
}

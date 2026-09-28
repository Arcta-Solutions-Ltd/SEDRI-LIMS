using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AddCodingListMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'addcodinglistmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            ListId: 77,
                            Value: '<:1:>',
                            Fixed: false,
                            Enabled: true,
                            Deleted: false,
                            DisplayOrder: 1
                        }
                     }";
        }
    }
}

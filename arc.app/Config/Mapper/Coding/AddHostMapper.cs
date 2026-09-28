using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AddHostMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'addhostmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            ListId: 78,
                            Value: '<:1:>',
                            Enabled: true,
                            Fixed: false,
                            Deleted: false,
                            DisplayOrder: 1
                        }
                     }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AddIqcTestProfileMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'addiqctestprofilemapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            ListId: 117,
                            Value: '<:1:>',
                            Fixed: false,
                            Enabled: true
                        }
                     }";
        }
    }
}

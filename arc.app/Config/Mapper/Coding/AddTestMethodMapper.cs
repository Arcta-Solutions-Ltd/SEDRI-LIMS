using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AddTestMethodMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'addtestmethodmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            ListId: 79,
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

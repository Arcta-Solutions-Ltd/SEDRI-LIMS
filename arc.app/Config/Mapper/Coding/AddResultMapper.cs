using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AddResultMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'addresultmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            ListId: 80,
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

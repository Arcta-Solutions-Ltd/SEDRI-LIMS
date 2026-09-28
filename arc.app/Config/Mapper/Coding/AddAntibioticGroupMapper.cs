using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AddAntibioticGroupMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'addantibioticgroupmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            ListId: 82,
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

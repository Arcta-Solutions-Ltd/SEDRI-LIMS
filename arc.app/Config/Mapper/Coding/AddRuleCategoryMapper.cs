using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AddRuleCategoryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'addrulecategorymapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            ListId: 129,
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

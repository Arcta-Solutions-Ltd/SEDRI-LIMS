using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AddTableMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'addtablemapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'ParentId', Value: 'ParentId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'Description', Value: 'Description' }
                        ],
                        'Target' : { 
                            Name: '<:1:>',
                            ParentId: '<:2:>',
                            Description: '<:3:>',
                            Grouping: 'Custom',
                            Common: true
                        }
                     }";
        }
    }
}

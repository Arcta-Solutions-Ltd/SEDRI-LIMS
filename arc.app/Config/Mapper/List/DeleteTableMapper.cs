using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class DeleteTableMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'deletetablemapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'ListId', Value: 'ListId' }
                        ],
                        'Target' : { 
                            Id: '<:1:>',
                            Grouping: 'Custom'
                        }
                     }";
        }
    }
}

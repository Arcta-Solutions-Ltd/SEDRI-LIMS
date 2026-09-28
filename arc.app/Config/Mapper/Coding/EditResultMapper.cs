using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class EditResultMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'editresultmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'ResultId', Value: 'ResultId' }
                        ],
                        'Target': { 
                            ListId: 80,
                            Id: '<:2:>',
                            Value: '<:1:>'
                        }
                     }";
        }
    }
}

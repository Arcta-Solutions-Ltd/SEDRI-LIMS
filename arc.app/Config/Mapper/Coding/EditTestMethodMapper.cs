using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class EditTestMethodMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'edittestmethodmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'TestMethodId', Value: 'TestMethodId' }
                        ],
                        'Target' : { 
                            ListId: 79,
                            Id: '<:2:>',
                            Value: '<:1:>'
                        }
                     }";
        }
    }
}

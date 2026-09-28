using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class EditHostMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'edithostmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'HostId', Value: 'HostId' }
                        ],
                        'Target': { 
                            ListId: 78,
                            Id: '<:2:>',
                            Value: '<:1:>'
                        }
                     }";
        }
    }
}

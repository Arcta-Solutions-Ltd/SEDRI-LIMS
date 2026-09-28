using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class Day1BenchReadMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'day1benchreadmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'BenchReadDay1Action', Value: 'BenchReadDay1Action' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'StateId', Value: 'StateId' }
                        ],
                        'Target': 
                            {
                                'Id':'<:1:>',
                                'StateId':'<:3:>'
                            }
                     }";
        }
    }
}


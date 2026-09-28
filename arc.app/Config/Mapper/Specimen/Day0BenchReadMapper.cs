using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class Day0BenchReadMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'day0benchreadmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'ReceivedConditionId', Value: 'ReceivedConditionId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'SpecimenAppearanceId', Value: 'SpecimenAppearanceId' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'SpecimenWeight', Value: 'SpecimenWeight' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'BenchReadDay0Action', Value: 'BenchReadDay0Action' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'StateId', Value: 'StateId' }
                        ],
                        'Target': 
                            {
                                'Id':'<:1:>',
                                'ReceivedConditionId':'<:2:>',
                                'SpecimenAppearanceId':'<:3:>',
                                'SpecimenWeight':'<:4:>',
                                'StateId':'<:6:>'
                            }
                     }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class BiochemistryTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'biochemistrytestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'glucose', Value: 'glucose' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'protein', Value: 'protein' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                glucose:'<:2:>',
                                protein:'<:3:>',
                                printonreport:'<:4:>'
                            }
                     }";
        }
    }
}

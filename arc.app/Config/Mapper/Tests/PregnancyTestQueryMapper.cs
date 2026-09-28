using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class PregnancyTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'pregnancytestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'pregnancyid', Value: 'pregnancyid' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                pregnancyid:'<:2:>',
                                printonreport:'<:3:>'
                            }
                     }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class HPyloriAntigenTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'hpyloriantigentestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'antresultid', Value: 'antresultid' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                antresultid:'<:2:>',
                                printonreport:'<:3:>'
                            }
                     }";
        }
    }
}

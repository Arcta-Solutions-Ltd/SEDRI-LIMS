using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class BetalactamaseTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'betalactamasetestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'betalactamaseresultid', Value: 'betalactamaseresultid' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                betalactamaseresultid:'<:2:>',
                                printonreport:'<:3:>'
                            }
                     }";
        }
    }
}

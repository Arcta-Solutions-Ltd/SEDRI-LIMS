using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class CatalaseTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'catalasetestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'catalaseresultid', Value: 'catalaseresultid' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                catalaseresultid:'<:2:>',
                                printonreport:'<:3:>'
                            }
                     }";
        }
    }
}

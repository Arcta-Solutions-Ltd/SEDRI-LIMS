using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class OxidaseTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'oxidasetestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'oxidaseresultid', Value: 'oxidaseresultid' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                oxidaseresultid:'<:2:>',
                                printonreport:'<:3:>'
                            }
                     }";
        }
    }
}

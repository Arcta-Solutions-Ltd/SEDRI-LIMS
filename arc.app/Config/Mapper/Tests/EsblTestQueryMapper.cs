using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class EsblTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'esbltestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'esblresultid', Value: 'esblresultid' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                esblresultid:'<:2:>',
                                printonreport:'<:3:>'
                            }
                     }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class WrightsStainTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'wrightsstaintestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'wrightsstainresultId', Value: 'wrightsstainresultId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                wrightsstainresultId:'<:2:>',
                                printonreport:'<:3:>'
                            }
                     }";
        }
    }
}

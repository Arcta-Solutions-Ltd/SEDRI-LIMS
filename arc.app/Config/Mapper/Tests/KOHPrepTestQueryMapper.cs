using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class KOHPrepTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'kohpreptestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'kohresultId', Value: 'kohresultId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'kohFungalId', Value: 'kohFungalId' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                kohresultId:'<:2:>',
                                kohFungalId:'<:3:>',
                                printonreport:'<:4:>'
                            }
                     }";
        }
    }
}

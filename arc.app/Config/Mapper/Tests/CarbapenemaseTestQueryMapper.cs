using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class CarbapenemaseTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'carbapenemasetestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'carbapenemaseresultId', Value: 'carbapenemaseresultId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                carbapenemaseresultid:'<:2:>',
                                printonreport:'<:3:>'
                            }
                     }";
        }
    }
}

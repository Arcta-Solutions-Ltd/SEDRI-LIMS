using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class IndiaInkTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'indiainktestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'indiainkresult', Value: 'indiainkresult' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'positiveresult', Value: 'positiveresult' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                indiainkresult:'<:2:>',
                                positiveresult:'<:3:>',
                                printonreport:'<:4:>'
                            }
                     }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Mapper.Tests
{
    internal class JEVSerologyTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'jevserologytestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'jevserologyresultid', Value: 'jevserologyresultid' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                jevserologyresultid:'<:2:>',
                                printonreport:'<:3:>'
                            }
                     }";
        }
    }
}

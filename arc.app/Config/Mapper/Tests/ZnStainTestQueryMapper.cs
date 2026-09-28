using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class ZnStainTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'znstaintestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'AFBQuantity', Value: 'AFBQuantity' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                AFBQuantity:'<:2:>',
                                printonreport:'<:3:>'
                            }
                     }";
        }
    }
}

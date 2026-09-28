using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class CellCountTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'cellcounttestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'ccwbc', Value: 'ccwbc' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'ccrbc', Value: 'ccrbc' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'wbcqualitative', Value: 'wbcqualitative' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'rbcqualitative', Value: 'rbcqualitative' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'polymorphonuclear', Value: 'polymorphonuclear' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'mononuclear', Value: 'mononuclear' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                ccwbc:'<:2:>',
                                ccrbc:'<:3:>',
                                wbcqualitative: '<:4:>',
                                rbcqualitative: '<:5:>',
                                polymorphonuclear : '<:6:>',
                                mononuclear: '<:7:>',
                                printonreport:'<:8:>'
                            }
                     }";
        }
    }
}

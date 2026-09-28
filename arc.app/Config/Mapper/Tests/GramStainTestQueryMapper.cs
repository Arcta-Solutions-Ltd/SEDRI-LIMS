using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class GramStainTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'gramstaintestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'wbc', Value: 'wbc' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'epicells', Value: 'epicells' },
                            { Key: '<:4:>', Type: 'Repeater' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                wbc:'<:2:>',
                                epicells:'<:3:>',
                                organismgrid: '<:4:>',
                                printonreport:'<:5:>'
                            },
                        Repeaters: [
                            { 
                                Name: '<:4:>', 
                                Rules: [
                                    { Key: '<:5:>', Type: 'Mapping', Source: 'organism', Value: 'organism' },
                                    { Key: '<:6:>', Type: 'Mapping', Source: 'wbclist', Value: 'wbclist' }
                                ],
                                Target: '{\'organism\': \'<:5:>\', \'wbclist\': \'<:6:>\'}'
                            }
                        ]
                     }";
        }
    }
}

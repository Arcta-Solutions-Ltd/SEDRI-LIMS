using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class MicroscopyTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'microscopytestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'epitheliumId', Value: 'epitheliumId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'bacteriaId', Value: 'bacteriaId' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'yeastId', Value: 'yeastId' },
                            { Key: '<:5:>', Type: 'Repeater' },
                            { Key: '<:6:>', Type: 'Repeater' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                epitheliumId:'<:2:>',
                                bacteriaId:'<:3:>',
                                yeastId:'<:4:>',
                                crystalgrid: '<:5:>',
                                castgrid: '<:6:>',
                                printonreport:'<:7:>'
                            },
                        Repeaters: [
                            { 
                                Name: '<:5:>', 
                                Rules: [
                                    { Key: '<:7:>', Type: 'Mapping', Source: 'crystalseen', Value: 'crystalseen' },
                                    { Key: '<:8:>', Type: 'Mapping', Source: 'crystal', Value: 'crystal' }
                                ],
                                Target: '{\'crystal\': \'<:8:>\', \'crystalseen\': \'<:7:>\'}'
                            },
                            { 
                                Name: '<:6:>', 
                                Rules: [
                                    { Key: '<:9:>', Type: 'Mapping', Source: 'castseen', Value: 'castseen' },
                                    { Key: '<:10:>', Type: 'Mapping', Source: 'cast', Value: 'cast' }
                                ],
                                Target: '{\'cast\': \'<:10:>\', \'castseen\': \'<:9:>\'}'
                            }
                        ]
                     }";
        }
    }
}

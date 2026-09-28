using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class WetPrepTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'wetpreptestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'wbcwetprep', Value: 'wbcwetprep' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'rbcwetprep', Value: 'rbcwetprep' },
                            { Key: '<:4:>', Type: 'Repeater' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                wbcwetprep:'<:2:>',
                                rbcwetprep:'<:3:>',
                                parasitegrid: '<:4:>',
                                printonreport:'<:5:>'
                            },
                        Repeaters: [
                            { 
                                Name: '<:4:>', 
                                Rules: [
                                    { Key: '<:5:>', Type: 'Mapping', Source: 'parasite', Value: 'parasite' },
                                    { Key: '<:6:>', Type: 'Mapping', Source: 'parasitetype', Value: 'parasitetype' },
                                    { Key: '<:7:>', Type: 'Mapping', Source: 'foundparasite', Value: 'foundparasite' }
                                ],
                                Target: '{\'parasite\': \'<:5:>\', \'parasitetype\': \'<:6:>\', \'foundparasite\': \'<:7:>\'}'
                            }
                        ]
                     }";
        }
    }
}

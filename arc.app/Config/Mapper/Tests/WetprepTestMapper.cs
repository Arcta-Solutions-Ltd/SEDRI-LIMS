using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class WetprepTestMapper : IDefinition
    {
        public string Get()
        {
            var testResults = @"{\'wbcwetprep\':\'<:1:>\',\'rbcwetprep\':\'<:2:>\',\'parasitegrid\': \'<:3:>\',\'printonreport\':\'<:6:>\'}";

            return @"{  
                        'Name': 'wetpreptestmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'wbcwetprep', Value: 'wbcwetprep' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'rbcwetprep', Value: 'rbcwetprep' },
                            { Key: '<:3:>', Type: 'Repeater' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'none', Value: '<:now:>' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'id', Value: 'id' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                'Id':'<:5:>',
                                'TestResults':'" + testResults + @"',
                                'Status':'Complete',
                                'Completed': '<:4:>'
                            },
                        Repeaters: [
                            { 
                                Name: '<:3:>', 
                                Rules: [
                                    { Key: '<:6:>', Type: 'Mapping', Source: 'parasite', Value: 'parasite' },
                                    { Key: '<:7:>', Type: 'Mapping', Source: 'parasitetype', Value: 'parasitetype' },
                                    { Key: '<:8:>', Type: 'Mapping', Source: 'foundparasite', Value: 'foundparasite' }
                                ],
                                Target: '{\'parasite\': \'<:6:>\', \'parasitetype\': \'<:7:>\', \'foundparasite\': \'<:8:>\'}'
                            }
                        ]
                     }";
        }
    }
}

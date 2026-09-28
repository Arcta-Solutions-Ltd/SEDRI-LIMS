using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class MicroscopyTestMapper : IDefinition
    {
        public string Get()
        {
            var testResults = @"{\'epitheliumId\':\'<:1:>\',\'bacteriaId\':\'<:2:>\',\'yeastId\':\'<:3:>\',\'crystalgrid\': \'<:4:>\',\'castgrid\': \'<:5:>\',\'printonreport\':\'<:8:>\'}";

            return @"{  
                        'Name': 'microscopytestmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'epitheliumId', Value: 'epitheliumId' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'bacteriaId', Value: 'bacteriaId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'yeastId', Value: 'yeastId' },
                            { Key: '<:4:>', Type: 'Repeater' },
                            { Key: '<:5:>', Type: 'Repeater' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'none', Value: '<:now:>' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'id', Value: 'id' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                'Id':'<:7:>',
                                'TestResults':'" + testResults + @"',
                                'Status':'Complete',
                                'Completed': '<:6:>'
                            },
                        Repeaters: [
                            { 
                                Name: '<:4:>', 
                                Rules: [
                                    { Key: '<:8:>', Type: 'Mapping', Source: 'crystalseen', Value: 'crystalseen' },
                                    { Key: '<:9:>', Type: 'Mapping', Source: 'crystal', Value: 'crystal' }
                                ],
                                Target: '{\'crystalseen\': \'<:8:>\', \'crystal\': \'<:9:>\'}'
                            },
                            { 
                                Name: '<:5:>', 
                                Rules: [
                                    { Key: '<:10:>', Type: 'Mapping', Source: 'castseen', Value: 'castseen' },
                                    { Key: '<:11:>', Type: 'Mapping', Source: 'cast', Value: 'cast' }
                                ],
                                Target: '{\'castseen\': \'<:10:>\', \'cast\': \'<:11:>\'}'
                            }
                        ]
                     }";
        }
    }
}

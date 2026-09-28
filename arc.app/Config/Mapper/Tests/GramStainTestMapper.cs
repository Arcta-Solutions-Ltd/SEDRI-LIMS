using arc.app.Common;

namespace arc.app.Config.Mapper.Tests
{
    internal class GramStainTestMapper : IDefinition
    {
        public string Get()
        {
            var testResults = @"{\'wbc\':\'<:1:>\',\'epicells\':\'<:2:>\',\'organismgrid\': \'<:5:>\',\'printonreport\':\'<:8:>\'}";

            return @"{  
                        'Name': 'gramstaintestmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'wbc', Value: 'wbc' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'epicells', Value: 'epicells' },
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
                                Name: '<:5:>', 
                                Rules: [
                                    { Key: '<:3:>', Type: 'Mapping', Source: 'organism', Value: 'organism' },
                                    { Key: '<:4:>', Type: 'Mapping', Source: 'wbclist', Value: 'wbclist' }
                                ],
                                Target: '{\'organism\': \'<:3:>\', \'wbclist\': \'<:4:>\'}'
                            }
                        ]
                     }";
        }
    }
}

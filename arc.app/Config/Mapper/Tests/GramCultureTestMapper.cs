using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class GramCultureTestMapper : IDefinition
    {
        public string Get()
        {
            var testResults = @"{\'gcwbc\':\'<:1:>\',\'gcepicells\':\'<:2:>\',\'gcorganismgrid\': \'<:5:>\',\'printonreport\':\'<:8:>\'}";

            return @"{  
                        'Name': 'gramculturetestmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:2:>', Type: 'Mapping', Source: 'gcepicells', Value: 'gcepicells' },
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
                                    { Key: '<:3:>', Type: 'Mapping', Source: 'gcorganism', Value: 'gcorganism' },
                                    { Key: '<:4:>', Type: 'Mapping', Source: 'gcwbclist', Value: 'gcwbclist' }
                                ],
                                Target: '{\'gcorganism\': \'<:3:>\', \'gcwbclist\': \'<:4:>\'}'
                            }
                        ]
                     }";
        }
    }
}

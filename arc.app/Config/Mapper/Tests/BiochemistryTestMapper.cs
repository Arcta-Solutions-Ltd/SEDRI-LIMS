using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class BiochemistryTestMapper : IDefinition
    {
        public string Get()
        {
            var testResults = @"{\'glucose\':\'<:2:>\',\'protein\':\'<:3:>\',\'printonreport\':\'<:5:>\'}";

            return @"{  
                        'Name': 'biochemistrytestmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'glucose', Value: 'glucose' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'protein', Value: 'protein' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'Id', Value: '<:now:>' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                'Id':'<:1:>',
                                'TestResults':'" + testResults + @"',
                                'Status':'Complete',
                                'Completed': '<:4:>'
                            }
                     }";
        }
    }
}

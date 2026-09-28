using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class PregnancyTestMapper : IDefinition
    {
        public string Get()
        {
            var testResults = @"{\'pregnancyId\':\'<:2:>\',\'printonreport\':\'<:4:>\'}";

            return @"{  
                        'Name': 'pregnancytestmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'pregnancyId', Value: 'pregnancyId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'Id', Value: '<:now:>' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                'Id':'<:1:>',
                                'TestResults':'" + testResults + @"',
                                'Status':'Complete',
                                'Completed': '<:3:>'
                            }
                     }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class JEVSerologyTestMapper : IDefinition
    {
        public string Get()
        {
            var testResults = @"{\'jevserologyResultId\':\'<:2:>\',\'printonreport\':\'<:4:>\'}";

            return @"{  
                        'Name': 'jevserologytestmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'jevserologyResultId', Value: 'jevserologyResultId' },
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

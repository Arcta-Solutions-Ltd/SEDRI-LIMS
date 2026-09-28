using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class BetalactamaseTestMapper : IDefinition
    {
        public string Get()
        {
            var testResults = @"{\'betalactamaseresultId\':\'<:2:>\',\'printonreport\':\'<:4:>\'}";

            return @"{  
                        'Name': 'betalactamasetestmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'BetalactamaseresultId', Value: 'BetalactamaseresultId' },
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

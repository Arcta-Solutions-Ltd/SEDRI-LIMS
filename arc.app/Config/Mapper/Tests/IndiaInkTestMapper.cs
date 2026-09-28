using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class IndiaInkTestMapper : IDefinition
    {
        public string Get()
        {
            var testResults = @"{\'indiainkresult\':\'<:2:>\',\'positiveresult\':\'<:3:>\',\'printonreport\':\'<:5:>\'}";

            return @"{  
                        'Name': 'indiainktestmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'indiainkresult', Value: 'indiainkresult' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'positiveresult', Value: 'positiveresult' },
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

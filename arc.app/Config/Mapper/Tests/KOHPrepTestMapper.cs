using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class KOHPrepTestMapper : IDefinition
    {
        public string Get()
        {
            var testResults = @"{\'kohresultId\':\'<:2:>\',\'kohFungalId\':\'<:3:>\',\'printonreport\':\'<:5:>\'}";

            return @"{  
                        'Name': 'kohpreptestmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': false,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'kohresultId', Value: 'kohresultId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'kohFungalId', Value: 'kohFungalId' },
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

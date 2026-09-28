using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class DipstickTestMapper : IDefinition
    {
        public string Get()
        {
            var testResults = @"{\'phId\': \'<:2:>\',\'specificGravityId\': \'<:3:>\',\'proteinId\': \'<:4:>\',\'ketonesId\': \'<:5:>\',\'glucoseId\': \'<:6:>\',\'bloodId\': \'<:7:>\',\'leucocytesId\': \'<:9:>\',\'nitritesId\': \'<:10:>\',\'printonreport\':\'<:11:>\'}";

            return @"{  
                        'Name': 'dipsticktestmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'phId', Value: 'phId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'specificGravityId', Value: 'specificGravityId' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'proteinId', Value: 'proteinId' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'ketonesId', Value: 'ketonesId' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'glucoseId', Value: 'glucoseId' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'bloodId', Value: 'bloodId' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'Id', Value: '<:now:>' },
                            { Key: '<:9:>', Type: 'Mapping', Source: 'leucocytesId', Value: 'leucocytesId' },
                            { Key: '<:10:>', Type: 'Mapping', Source: 'nitritesId', Value: 'nitritesId' },
                            { Key: '<:11:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                'Id':'<:1:>',
                                'TestResults':'" + testResults + @"',
                                'Status':'Complete',
                                'Completed': '<:8:>'
                            }
                     }";
        }
    }
}

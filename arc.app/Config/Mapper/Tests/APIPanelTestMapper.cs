using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class APIPanelTestMapper : IDefinition
    {
        public string Get()
        {
            var testResults = @"{\'apiidpanel\':\'<:2:>\',\'idprofile\':\'<:3:>\',\'percentageid\':\'<:4:>\',\'printonreport\':\'<:6:>\'}";

            return @"{  
                        'Name': 'apipaneltestmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'apiidpanel', Value: 'apiidpanel' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'idprofile', Value: 'idprofile' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'percentageid', Value: 'percentageid' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'Id', Value: '<:now:>' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                'Id':'<:1:>',
                                'TestResults':'" + testResults + @"',
                                'Status':'Complete',
                                'Completed': '<:5:>'
                            }
                     }";
        }
    }
}

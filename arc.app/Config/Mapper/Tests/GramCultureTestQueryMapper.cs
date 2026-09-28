using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class GramCultureTestQueryMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'gramculturetestquerymapper', 
                        'Type': 'Standard',
                        'JsonFields' : 'TestResults',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'gcepicells', Value: 'gcepicells' },
                            { Key: '<:4:>', Type: 'Repeater' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'printonreport', Value: 'printonreport' }
                        ],
                        'Target': 
                            {
                                Id:'<:1:>',
                                gcepicells:'<:3:>',
                                gcorganismgrid: '<:4:>',
                                printonreport:'<:5:>'
                            },
                        Repeaters: [
                            { 
                                Name: '<:4:>', 
                                Rules: [
                                    { Key: '<:5:>', Type: 'Mapping', Source: 'gcorganism', Value: 'gcorganism' },
                                    { Key: '<:6:>', Type: 'Mapping', Source: 'gcwbclist', Value: 'gcwbclist' }
                                ],
                                Target: '{\'gcorganism\': \'<:5:>\', \'gcwbclist\': \'<:6:>\'}'
                            }
                        ]
                     }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class ASTReportMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'astreportmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'TestType', Value: 'TestType' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Antibiotic', Value: 'Antibiotic' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'Susceptibility', Value: 'Susceptibility' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'AdditionalNotes', Value: 'AdditionalNotes' }
                        ],
                        'Target': 
                            {
                                Title: '',
                                Rows: [
                                    {
                                        Fields: [
                                            { Id: 'antibiotic', Label: '', Value: '<:2:>' },
                                            { Id: 'susceptibility', Label: '', NoLabel: true, Value: '<:3:>' },
                                            { Id: 'notes', Label: '', NoLabel: true, Value: '<:4:>' }
                                        ]
                                    }
                                ]
                            }
                     }";
        }
    }
}



using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class ASTReportHeaderMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'astreportheadermapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' }
                        ],
                        'Target': 
                            {
                                Title: '',
                                Rows: [
                                    {
                                        Fields: [
                                            { Id: 'antibiotic', Label: '', Value: 'Antibiotic', NoBox: true, Style: 'emphasis-centre' },
                                            { Id: 'susceptibility', Label: '', NoLabel: true, Value: 'Susceptibility', NoBox: true, Style: 'emphasis-centre' },
                                            { Id: 'notes', Label: '', NoLabel: true, Value: 'Notes', NoBox: true, Style: 'emphasis-centre' }
                                        ]
                                    }
                                ]
                            }
                     }";
        }
    }
}



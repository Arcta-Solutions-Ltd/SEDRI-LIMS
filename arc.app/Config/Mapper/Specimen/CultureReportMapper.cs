using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class CultureReportMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'culturereportmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'SpecimenOrganism', Value: 'SpecimenOrganism' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'SpecimenQuantity', Value: 'SpecimenQuantity' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'ESBL', Value: 'ESBL' }
                        ],
                        'Target': 
                            {
                                Title: '',
                                Padding: true,
                                Rows: [
                                    {
                                        Fields: [
                                            { Id: 'specimenorganism', Label: 'Organism', Value: '<:1:>' }
                                        ]
                                    },
                                    {
                                        Fields: [
                                            { Id: 'specimenquantity', Label: 'Quantity', Value: '<:2:>' },
                                            { Id: 'esbl', Label: 'ESBL', Value: '<:3:>' }
                                        ]
                                    },
                                    {
                                        Fields: [
                                            { Id: 'dummy', Label: '', Value: '', NoBox: true }
                                        ]
                                    }
                                ]
                            }
                     }";
        }
    }
}



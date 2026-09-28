using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class SpecimenReportHeaderMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'specimenreportheadermapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'AccessionNumber', Value: 'AccessionNumber' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'CollectionDate', Value: 'CollectionDate' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'SpecimenType', Value: 'SpecimenType' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'SpecimenSite', Value: 'SpecimenSite' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'State', Value: 'State' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'CollectionTime', Value: 'CollectionTime' }
                        ],
                        'Target': 
                            {
                                Type: 'Header',
                                Title: '',
                                Rows: [
                                    {
                                        Fields: [
                                            { Id: 'reportlocation', Label: '', NoLabel: true, Value: 'Main Hospital', NoBox: true, Style: 'heading2', WideValue: true }
                                        ]
                                    },
                                    {
                                        Fields: [
                                            { Id: 'reportname', Label: '', NoLabel: true, Value: 'Microbiology Laboratory Report', NoBox: true, Style: 'heading1', WideValue: true }
                                        ]
                                    },
                                    {
                                        Fields: [
                                            { Id: 'spectypeandsite', Label: '', NoLabel: true, Value: '<:3:> - <:4:>', NoBox: true, Style: 'heading3' }
                                        ]
                                    },
                                    {
                                        Fields: [
                                            { Id: 'accessionnumber', Label: 'Accession Number', Value: '<:1:>', Style: 'emphasis' },
                                            { Id: 'collectiondate', Label: 'Collection Date', Value: '<:2:>', Style: 'emphasis' },
                                            { Id: 'collectiontime', Label: 'Collection Time', Value: '<:6:>', Style: 'emphasis' }
                                        ]
                                    }
                                ]
                            }
                     }";
        }
    }
}



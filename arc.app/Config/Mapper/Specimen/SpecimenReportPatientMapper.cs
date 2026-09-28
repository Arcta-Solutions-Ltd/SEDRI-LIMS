using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class SpecimenReportPatientMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'specimenreportpatientmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'FirstName', Value: 'FirstName' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Surname', Value: 'Surname' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'DateOfBirth', Value: 'DateOfBirth' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'PatientLocation', Value: 'PatientLocation' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'Ward', Value: 'Ward' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'PatientRef', Value: 'PatientRef' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'Gender', Value: 'Gender' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'Province', Value: 'Province' },
                            { Key: '<:9:>', Type: 'Mapping', Source: 'District', Value: 'District' },
                            { Key: '<:10:>', Type: 'Mapping', Source: 'SubDistrict', Value: 'SubDistrict' }
                        ],
                        'Target': 
                            {
                                Title: 'Patient Details',
                                Rows: [
                                    {
                                        Fields: [
                                            { Id: 'firstname', Label: 'First Name', Value: '<:1:>' },
                                            { Id: 'PatientRef', Label: 'Patient Ref', Value: '<:6:>' }
                                        ]
                                    },
                                    {
                                        Fields: [
                                            { Id: 'surname', Label: 'Surname', Value: '<:2:>' }
                                        ]
                                    },
                                    {
                                        Fields: [
                                            { Id: 'gender', Label: 'Gender', Value: '<:7:>' }
                                        ]
                                    },
                                    {
                                        Fields: [
                                            { Id: 'dateofbirth', Label: 'Date of Mirth', Value: '<:3:>' }
                                        ]
                                    },
                                    {
                                        Fields: [
                                            { Id: 'patientlocation', Label: 'Location', Value: '<:4:>' }
                                        ]
                                    },
                                    {
                                        Fields: [
                                            { Id: 'ward', Label: 'Ward', Value: '<:5:>' }
                                        ]
                                    },
                                    {
                                        Fields: [
                                            { Id: 'province', Label: 'Province', Value: '<:8:>' },
                                            { Id: 'district', Label: 'District', Value: '<:9:>' }
                                        ]
                                    },
                                    {
                                        Fields: [
                                            { Id: 'subdistrict', Label: 'Subdistrict', Value: '<:10:>' }
                                        ]
                                    }
                                ]
                            }
                     }";
        }
    }
}

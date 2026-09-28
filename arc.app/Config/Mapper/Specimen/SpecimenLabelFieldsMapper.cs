using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class SpecimenLabelFieldsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'specimenlabelfieldsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'AdmissionDate', Value: 'AdmissionDate' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'ClinicalContactNo', Value: 'ClinicalContactNo' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'ExistingBarcode', Value: 'ExistingBarcode' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'BottleOnlyWeight', Value: 'BottleOnlyWeight' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'BloodAndBottleWeight', Value: 'BloodAndBottleWeight' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'CollectionDate', Value: 'CollectionDate' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'ReceivedDate', Value: 'ReceivedDate' },
                            { Key: '<:9:>', Type: 'Mapping', Source: 'FirstName', Value: 'FirstName' },
                            { Key: '<:10:>', Type: 'Mapping', Source: 'Surname', Value: 'Surname' },
                            { Key: '<:11:>', Type: 'Mapping', Source: 'PatientRef', Value: 'PatientRef' },
                            { Key: '<:12:>', Type: 'Mapping', Source: 'OrganisationName', Value: 'OrganisationName' },
                            { Key: '<:13:>', Type: 'Mapping', Source: 'SpecimenType', Value: 'SpecimenType' },
                            { Key: '<:14:>', Type: 'Mapping', Source: 'SpecimenSite', Value: 'SpecimenSite' },
                            { Key: '<:15:>', Type: 'Mapping', Source: 'ReceivedCondition', Value: 'ReceivedCondition' },
                            { Key: '<:16:>', Type: 'Mapping', Source: 'Diagnosis', Value: 'Diagnosis' },
                            { Key: '<:17:>', Type: 'Mapping', Source: 'AccessionNumber', Value: 'AccessionNumber' }
                        ],
                        'Target':
                            {
                                'admissiondate':'<:1:>',
                                'clinicalcontactno':'<:2:>',
                                'existingbarcode':'<:3:>',
                                'bottleonlyweight':'<:4:>',
                                'bloodandbottleweight':'<:5:>',
                                'collectiondate':'<:6:>',
                                'receiveddate':'<:7:>',
                                'firstname':'<:9:>',
                                'surname':'<:10:>',
                                'patientref':'<:11:>',
                                'organisationid':'<:12:>',
                                'specimentypeid':'<:13:>',
                                'specimensiteid':'<:14:>',
                                'receivedconditionid':'<:15:>',
                                'diagnosisid':'<:16:>',
                                'accessionnumber':'<:17:>',
                            }
                     }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class SpecimenLabelAvailableFieldsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'specimenlabelavailablefieldsmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
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
                            { Key: '<:12:>', Type: 'Mapping', Source: 'OrganisationId', Value: 'OrganisationId' },
                            { Key: '<:13:>', Type: 'Mapping', Source: 'SpecimenTypeId', Value: 'SpecimenTypeId' },
                            { Key: '<:14:>', Type: 'Mapping', Source: 'SpecimenSiteId', Value: 'SpecimenSiteId' },
                            { Key: '<:15:>', Type: 'Mapping', Source: 'ReceivedConditionId', Value: 'ReceivedConditionId' },
                            { Key: '<:16:>', Type: 'Mapping', Source: 'DiagnosisId', Value: 'DiagnosisId' },
                            { Key: '<:17:>', Type: 'Mapping', Source: 'AccessionNumber', Value: 'AccessionNumber' }
                        ],
                        'Target':
                            {
                                'AdmissionDate':'<:1:>',
                                'ClinicalContactNo':'<:2:>',
                                'ExistingBarcode':'<:3:>',
                                'BottleOnlyWeight':'<:4:>',
                                'BloodAndBottleWeight':'<:5:>',
                                'CollectionDate':'<:6:>',
                                'ReceivedDate':'<:7:>',
                                'FirstName':'<:9:>',
                                'Surname':'<:10:>',
                                'PatientRef':'<:11:>',
                                'OrganisationId':'<:12:>',
                                'SpecimenTypeId':'<:13:>',
                                'SpecimenSiteId':'<:14:>',
                                'ReceivedConditionId':'<:15:>',
                                'DiagnosisId':'<:16:>',
                                'AccessionNumber':'<:17:>'
                            }
                     }";
        }
    }
}


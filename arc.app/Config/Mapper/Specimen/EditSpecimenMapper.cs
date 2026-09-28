using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class EditSpecimenMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'editspecimenmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'id', Value: 'id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'patientlocationid', Value: 'patientlocationid' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'organisationid', Value: 'organisationid' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'admissiondate', Value: 'admissiondate' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'diagnosisid', Value: 'diagnosisid' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'clinicalcontactno', Value: 'clinicalcontactno' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'AntibioticsInLast24hrsId', Value: 'AntibioticsInLast24hrsId' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'TempInLast24hrsId', Value: 'TempInLast24hrsId' },
                            { Key: '<:9:>', Type: 'Mapping', Source: 'AdditionalClinicalInformation', Value: 'AdditionalClinicalInformation' },
                            { Key: '<:10:>', Type: 'Mapping', Source: 'specimentypeid', Value: 'specimentypeid' },
                            { Key: '<:11:>', Type: 'Mapping', Source: 'specimensiteid', Value: 'specimensiteid' },
                            { Key: '<:12:>', Type: 'Mapping', Source: 'accessionnumber', Value: 'accessionnumber' },
                            { Key: '<:13:>', Type: 'Mapping', Source: 'existingbarcode', Value: 'existingbarcode' },
                            { Key: '<:14:>', Type: 'Mapping', Source: 'FurtherInformation', Value: 'FurtherInformation' },
                            { Key: '<:15:>', Type: 'Mapping', Source: 'bottleonlyweight', Value: 'bottleonlyweight' },
                            { Key: '<:16:>', Type: 'Mapping', Source: 'receivedconditionid', Value: 'receivedconditionid' },
                            { Key: '<:17:>', Type: 'Mapping', Source: 'specimenappearanceid', Value: 'specimenappearanceid' },
                            { Key: '<:18:>', Type: 'Mapping', Source: 'collectiondate', Value: 'collectiondate' },
                            { Key: '<:19:>', Type: 'Mapping', Source: 'receiveddate', Value: 'receiveddate' },
                            { Key: '<:20:>', Type: 'Mapping', Source: 'collectiontime', Value: 'collectiontime' },
                            { Key: '<:21:>', Type: 'Mapping', Source: 'receivedtime', Value: 'receivedtime' },
                            { Key: '<:22:>', Type: 'Mapping', Source: 'TestCategoryId', Value: 'TestCategoryId' },
                            { Key: '<:23:>', Type: 'Mapping', Source: 'CultureTypeCategoryId', Value: 'CultureTypeCategoryId' },
                            { Key: '<:24:>', Type: 'Mapping', Source: 'BloodAndBottleWeight', Value: 'BloodAndBottleWeight' },
                            { Key: '<:25:>', Type: 'Mapping', Source: 'AgeYears', Value: 'AgeYears' },
                            { Key: '<:26:>', Type: 'Mapping', Source: 'AgeMonths', Value: 'AgeMonths' },
                            { Key: '<:27:>', Type: 'Mapping', Source: 'AgeDays', Value: 'AgeDays' },
                            { Key: '<:28:>', Type: 'Mapping', Source: 'AgeHours', Value: 'AgeHours' }
                        ],
                        'Target': 
                            {
                                'Id':'<:1:>',
                                'patientlocationid':'<:2:>',
                                'organisationid':'<:3:>',
                                'admissiondate':'<:4:>',
                                'diagnosisid':'<:5:>',
                                'clinicalcontactno':'<:6:>',
                                'AntibioticsInLast24hrsId':'<:7:>',
                                'TempInLast24hrsId':'<:8:>',
                                'AdditionalClinicalInformation':'<:9:>',
                                'specimentypeid':'<:10:>',
                                'specimensiteid':'<:11:>',
                                'accessionnumber':'<:12:>',
                                'existingbarcode':'<:13:>',
                                'FurtherInformation':'<:14:>',
                                'bottleonlyweight':'<:15:>',
                                'receivedconditionid':'<:16:>',
                                'specimenappearanceid':'<:17:>',
                                'collectiondate':'<:18:>',
                                'receiveddate':'<:19:>',
                                'collectiontime':'<:20:>',
                                'receivedtime':'<:21:>',
                                'TestCategoryId':'<:22:>',
                                'CultureTypeCategoryId':'<:23:>',
                                'BloodAndBottleWeight':'<:24:>',
                                'AgeYears':'<:25:>',
                                'AgeMonths':'<:26:>',
                                'AgeDays':'<:27:>',
                                'AgeHours':'<:28:>'
                            }
                     }";
        }
    }
}

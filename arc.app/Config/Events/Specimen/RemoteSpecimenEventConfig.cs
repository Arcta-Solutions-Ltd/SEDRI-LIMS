using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class RemoteSpecimenEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'remotespecimen', 
                        Description: '@SpeAddB@',
                        EventType : 'specialadddata', 
                        Topic : 'Specimen', 
                        TableName: 'Specimen',
                        DefaultView: 'specimens',
                        ValidationRules: [
                            { field: 'CollectionDate', rule: 'required', message: '@SpeCol@'},
                            { field: 'SpecimenTypeId', rule: 'required', message: '@SpeSpeN@'},
                            { field: 'DiagnosisId', rule: 'required', message: '@SpeAdi@'}
                        ],
                        Lists: 'Gender, Diagnosis, DistrictId, ProvinceId, SpecimenSite, SpecimenType, SubdistrictId, PatientLocation',
                        Display: [
                            { Label: 'action', Translation: '@SpeImm@', List: 'Yes' },
                            { Label: 'patientref', Translation: '@PatPatB@', List: 'No' },
                            { Label: 'firstname', Translation: '@UseFir@', List: 'No' },
                            { Label: 'surname', Translation: '@PatSurA@', List: 'No' },
                            { Label: 'gender', Translation: '@PatGenA@', List: 'Yes' },
                            { Label: 'dateofbirth', Translation: '@PatDat@', List: 'No', Date: true },
                            { Label: 'subdistrictid', Translation: '@PatSub@', List: 'Yes' },
                            { Label: 'districtid', Translation: '@PatDis@', List: 'Yes' },
                            { Label: 'provinceid', Translation: '@PatPro@', List: 'Yes' },
                            { Label: 'telephonenumber', Translation: '@PatTel@', List: 'No' },
                            { Label: 'patientlocation', Translation: '@PatPatJ@', List: 'Yes' },
                            { Label: 'antibioticsinlast24hrsid', Translation: '@CusAnt@', List: 'Yes' },
                            { Label: 'tempinlast24hrsid', Translation: '@CusTem@', List: 'Yes' },
                            { Label: 'diagnosisid', Translation: '@GenDia@', List: 'Yes' },
                            { Label: 'existingbarcode', Translation: '@SpeExi@', List: 'Yes' },
                            { Label: 'additionalclinicalinformation', Translation: '@CusAdd@', List: 'No' },
                            { Label: 'specimensiteid', Translation: '@SpeSpeC@', List: 'Yes' },
                            { Label: 'specimentypeid', Translation: '@SpeSpeB@', List: 'Yes' },
                            { Label: 'furtherinformation', Translation: '@CusFur@', List: 'No' },
                            { Label: 'collectiondate', Translation: '@SpeColC@', List: 'No', Date: true },
                            { Label: 'collectiontime', Translation: '@SpeColD@', List: 'No' },
                            { Label: 'ageyears', Translation: '@SpeAge@', List: 'No' },
                            { Label: 'agemonths', Translation: '@SpeAge@', List: 'No' },
                            { Label: 'agedays', Translation: '@SpeAge@', List: 'No' },
                            { Label: 'agehours', Translation: '@SpeAge@', List: 'No' },
                            { Label: 'requestculturetests', Translation: '@CusReq@', List: 'No' },
                            { Label: 'melioidosisculturetests', Translation: '@CusMel@', List: 'No' },
                            { Label: 'organisationid', Translation: '@GenWar@', List: 'No' },
                            { Label: 'clinicalcontactno', Translation: '@PatCli@', List: 'No' }
                        ]
                    }";
        }

    }
}

                            //{ field: 'OrganisationId', rule: 'required', message: '@SpeAn@'},
                            //{ field: 'LaboratoryId', rule: 'required', message: '@SpeAnA@'},

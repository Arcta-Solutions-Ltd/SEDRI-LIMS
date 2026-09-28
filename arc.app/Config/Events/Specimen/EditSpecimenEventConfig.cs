using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditSpecimenEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editspecimen', 
                        Description: '@SpeEdiB@',
                        EventType : 'special',
                        Topic : 'Specimen', 
                        TableName: 'Specimen',
                        Mapping: 'editspecimenmapper',
                        StringFields: 'TestCategoryId,CultureTypeCategoryId,AdditionalClinicalInformation,FurtherInformation',
                        ValidationRules: [
                            { field: 'CollectionDate', rule: 'required', message: '@SpeCol@'},
                            { field: 'ReceivedDate', rule: 'required', message: '@SpeRec@'},
                            { field: 'ReceivedTime', rule: 'required', message: '@SpeRecA@'},
                            { field: 'SpecimenTypeId', rule: 'required', message: '@SpeSpeN@'},
                            { field: 'DiagnosisId', rule: 'required', message: '@SpeAdi@'}
                        ],
                        Display: [
                            { Label: 'action', Translation: '@SpeImm@', List: 'Yes' },
                            { Label: 'patientref', Translation: '@PatPatB@', List: 'No' },
                            { Label: 'firstname', Translation: '@UseFir@', List: 'No' },
                            { Label: 'surname', Translation: '@PatSurA@', List: 'No' },
                            { Label: 'gender', Translation: '@PatGenA@', List: 'Yes' },
                            { Label: 'dateofbirth', Translation: '@PatDat@', List: 'No', Date: true },
                            { Label: 'addressline1', Translation: '@PatAddD@', List: 'No' },
                            { Label: 'addressline2', Translation: '@PatAddE@', List: 'No' },
                            { Label: 'subdistrictid', Translation: '@PatSub@', List: 'Yes' },
                            { Label: 'districtid', Translation: '@PatDis@', List: 'Yes' },
                            { Label: 'provinceid', Translation: '@PatPro@', List: 'Yes' },
                            { Label: 'zipcode', Translation: '@PatZip@', List: 'No' },
                            { Label: 'telephonenumber', Translation: '@PatTel@', List: 'No' },
                            { Label: 'patientlocation', Translation: '@PatPatJ@', List: 'Yes' },
                            { Label: 'antibioticsinlast24hrsid', Translation: '@CusAnt@', List: 'Yes' },
                            { Label: 'tempinlast24hrsid', Translation: '@CusTem@', List: 'Yes' },
                            { Label: 'diagnosisid', Translation: '@GenDia@', List: 'Yes' },
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
                            { Label: 'receiveddate', Translation: '@SpeRecD@', List: 'No', Date: true },
                            { Label: 'receivedtime', Translation: '@SpeRecE@', List: 'No' },
                            { Label: 'receivedconditionid', Translation: '@SpeSpeE@', List: 'Yes' },
                            { Label: 'organisationid', Translation: '@GenWar@', List: 'No' },
                            { Label: 'clinicalcontactno', Translation: '@PatCli@', List: 'No' },
                            { Label: 'specimenappearanceid', Translation: '@SpeSpeF@', List: 'Yes' },
                            { Label: 'testcategoryid', Translation: '@GenTesG@', List: 'Yes' },
                            { Label: 'culturetypecategoryid', Translation: '@GenCul@', List: 'Yes' }
                        ]
                    }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class ReceivedSpecimenEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'newreceivedspecimen', 
                        Description: '@SpeAddA@',
                        EventType : 'specialadddata',
                        Topic : 'Specimen',
                        TableName: 'Specimen',
                        DefaultView: 'specimens',
                        TableExceptions: [
                            { field: 'ManufacturersBarcode', table: 'Culture' }
                        ],
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
                            { Label: 'diagnoses', Translation: '@GenDia@', List: 'Yes' },
                            { Label: 'specimensite', Translation: '@SpeSpeC@', List: 'Yes' },
                            { Label: 'specimentype', Translation: '@SpeSpeB@', List: 'Yes' },
                            { Label: 'collectiondate', Translation: '@SpeColC@', List: 'No', Date: true },
                            { Label: 'collectiontime', Translation: '@SpeColD@', List: 'No' },
                            { Label: 'ageyears', Translation: '@SpeAge@', List: 'No' },
                            { Label: 'agemonths', Translation: '@SpeAge@', List: 'No' },
                            { Label: 'agedays', Translation: '@SpeAge@', List: 'No' },
                            { Label: 'agehours', Translation: '@SpeAge@', List: 'No' },
                            { Label: 'receiveddate', Translation: '@SpeRecD@', List: 'No', Date: true },
                            { Label: 'receivedtime', Translation: '@SpeRecE@', List: 'No' },
                            { Label: 'receivedcondition', Translation: '@SpeSpeE@', List: 'Yes' },
                            { Label: 'organisationid', Translation: '@GenWar@', List: 'No' },
                            { Label: 'clinicalcontactno', Translation: '@PatCli@', List: 'No' },
                            { Label: 'specimenappearance', Translation: '@SpeSpeF@', List: 'Yes' }
                        ]
                    }";
        }

    }
}

//{ field: 'OrganisationId', rule: 'required', message: '@SpeAn@'},

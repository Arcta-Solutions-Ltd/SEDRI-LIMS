using arc.app.Common;

namespace arc.app.Config.Events
{
    /// <summary>
    /// Event configuration for the Neoshield neonatal specimen request. Field routing to Patient,
    /// Admission, Request and Specimen is driven by each page's TableName in form configuration.
    /// </summary>
    internal class NeoshieldSpecimenEventConfig : IDefinition
    {
        /// <summary>
        /// Retrieves the event definition.
        /// </summary>
        /// <returns>A string containing the event definition in JSON format.</returns>
        public string Get()
        {
            return @"{ 
                        EventName: 'neoshieldspecimen', 
                        Description: '@NeoNeoA@',
                        EventType : 'specialadddata',
                        Topic : 'Specimen',
                        TableName: 'Specimen',
                        DefaultView: 'specimens',
                        TableExceptions: [
                            { field: 'ManufacturersBarcode', table: 'Culture' }
                        ],
                        ValidationRules: [
                            { field: 'SpecimenTypeId', rule: 'required', message: '@NeoValSpeTyp@'},
                            { field: 'CollectionDate', rule: 'required', message: '@SpeCol@'}
                        ],
                        Display: [
                            { Label: 'patientref', Translation: '@PatPatB@', List: 'No' },
                            { Label: 'firstname', Translation: '@NeoBabFir@', List: 'No' },
                            { Label: 'surname', Translation: '@NeoBabLas@', List: 'No' },
                            { Label: 'gender', Translation: '@PatGenA@', List: 'Yes' },
                            { Label: 'dateofbirth', Translation: '@PatDat@', List: 'No', Date: true },
                            { Label: 'motherpatientref', Translation: '@NeoMotRef@', List: 'No' },
                            { Label: 'birthweight', Translation: '@NeoBirWei@', List: 'No' },
                            { Label: 'inbornoutborn', Translation: '@NeoInbOut@', List: 'Yes' },
                            { Label: 'dateofadmission', Translation: '@NeoAdmDat@', List: 'No', Date: true },
                            { Label: 'timeofadmission', Translation: '@NeoAdmTim@', List: 'No' },
                            { Label: 'wardid', Translation: '@NeoWar@', List: 'Yes' },
                            { Label: 'cotid', Translation: '@NeoCot@', List: 'Yes' },
                            { Label: 'urgencyid', Translation: '@NeoUrg@', List: 'Yes' },
                            { Label: 'indicationid', Translation: '@NeoInd@', List: 'Yes' },
                            { Label: 'currentweight', Translation: '@NeoCurWei@', List: 'No' },
                            { Label: 'recentsurgeryid', Translation: '@NeoRecSur@', List: 'Yes' },
                            { Label: 'respiratorysupportid', Translation: '@NeoResSup@', List: 'Yes' },
                            { Label: 'specimentype', Translation: '@SpeSpeB@', List: 'Yes' },
                            { Label: 'specimensite', Translation: '@SpeSpeC@', List: 'Yes' },
                            { Label: 'collectiondate', Translation: '@SpeColC@', List: 'No', Date: true },
                            { Label: 'collectiontime', Translation: '@SpeColD@', List: 'No' },
                            { Label: 'bodysideid', Translation: '@NeoBodSid@', List: 'Yes' },
                            { Label: 'collectionmethodid', Translation: '@NeoColMet@', List: 'Yes' },
                            { Label: 'collectedbyid', Translation: '@NeoColBy@', List: 'Yes' },
                            { Label: 'bottletypeid', Translation: '@NeoBotTyp@', List: 'Yes' },
                            { Label: 'bottleonlyweight', Translation: '@NeoBotWei@', List: 'No' },
                            { Label: 'estimatedbloodvolume', Translation: '@NeoEstVol@', List: 'No' }
                        ]
                    }";
        }
    }
}

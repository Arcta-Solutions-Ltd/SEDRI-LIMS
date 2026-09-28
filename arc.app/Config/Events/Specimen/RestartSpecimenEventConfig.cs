using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class RestartSpecimenEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'restartspecimen',
                        Description: '@SpeRes@',
                        EventType : 'editdata',
                        Topic : 'Specimen',
                        TableName: 'Specimen',
                        ValidationRules: [
                            { field: 'StateId', rule: 'required', message: '@SpeAsp@'}
                        ],
                        Display: [
                            { Label: 'specimenstate', Translation: '@SpeSpeQ@', List: 'No' }
                        ]
                    }";
        }
    }
}

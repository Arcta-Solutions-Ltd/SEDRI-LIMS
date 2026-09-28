using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class RejectSpecimenEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'rejectspecimen',
                        Description: '@SpeRej@',
                        EventType : 'editdata',
                        Topic : 'Specimen',
                        Mapping : 'rejectspecimenmapper',
                        TableName: 'Specimen',
                        ValidationRules: [
                            { field: 'ReceivedConditionId', rule: 'required', message: '@SpeRecF@'}
                        ],
                        Display: [
                            { Label: 'receivedcondition', Translation: '@SpeSpeE@', List: 'Yes' },
                            { Label: 'rejectionreason', Translation: '@SpeRea@', List: 'No' }
                        ]
                    }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class SpecimenCancelRequestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'specimencancelrequest', 
                        Description: '@SpeCan@',
                        EventType : 'editdata', 
                        Topic : 'Specimen', 
                        TableName: 'Specimen',
                        Display: [
                            { Label: 'accessionnumber', Translation: '@SpeAcc@', List: 'No' },
                            { Label: 'rejectionreason', Translation: '@SpeCanC@', List: 'No' }
                        ]
                    }";
        }
    }
}

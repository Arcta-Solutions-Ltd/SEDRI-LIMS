using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class Day0BenchReadEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'Day0BenchRead',
                        Description: '@SpeBat@',
                        EventType : 'editdata',
                        Mapping : 'day0benchreadmapper',
                        Topic : 'Specimen',
                        TableName: 'Specimen',
                        Display: [
                            { Label: 'receivedconditionid', Translation: '@SpeSpeE@', List: 'Yes' },
                            { Label: 'specimenappearanceid', Translation: '@SpeSpeF@', List: 'Yes' },
                            { Label: 'specimenweight', Translation: '@SpeSpeD@', List: 'No' },
                            { Label: 'BenchReadDay0Action', Translation: '@GenAct@', List: 'Yes' }
                        ]
                    }";
        }
    }
}

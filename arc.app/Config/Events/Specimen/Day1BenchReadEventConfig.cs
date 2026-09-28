using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class Day1BenchReadEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'Day1BenchRead',
                        Description: '@SpeBatA@',
                        EventType: 'special',
                        Topic: 'Specimen',
                        TableName: 'Specimen',
                        Display:
                        [
                            { Label: 'BenchReadDay1Action', Translation: '@GenAct@', List: 'Yes' }
                        ]
                    }";
        }
    }
}


//EventName: 'Day1BenchRead',
//                        Description: '@SpeBatA@',
//                        EventType: 'editdata',
//                        Mapping: 'day1benchreadmapper',
//                        Topic: 'Specimen',
//                        TableName: 'Specimen',
//                        Display:
//[
//                            { Label: 'BenchReadDay1Action', Translation: '@GenAct@', List: 'No' }
//                        ]
 






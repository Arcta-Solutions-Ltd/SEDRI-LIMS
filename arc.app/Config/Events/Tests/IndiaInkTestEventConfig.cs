using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class IndiaInkTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'IndiaInkTest', 
                        Description: '@TesInd@',
                        EventType : 'editdata', 
                        Mapping: 'indiainktestmapper',
                        StringFields: 'TestResults',
                        Topic : 'Tests', 
                        TableName: 'Tests',
                        Display: [
                            { Label: 'indiainkresult', Translation: '@TesIndA@', List: 'Yes' },
                            { Label: 'positiveresult', Translation: '@TesPos@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ]
                    }";
        }
    }
}

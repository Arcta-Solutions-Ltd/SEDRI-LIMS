using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class JEVSerologyTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'JEVSerologyTest', 
                        Description: '@TesJev@',
                        EventType : 'editdata', 
                        Mapping: 'jevserologytestmapper',
                        StringFields: 'TestResults',
                        Topic : 'Tests', 
                        TableName: 'Tests',
                        Display: [
                            { Label: 'jevserologyResultId', Translation: '@TesTesA@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ],
                        ValidationRules: [
                            { field: 'jevserologyResultId', rule: 'required', message: '@GenResA@'}
                        ]
                    }";
        }
    }
}

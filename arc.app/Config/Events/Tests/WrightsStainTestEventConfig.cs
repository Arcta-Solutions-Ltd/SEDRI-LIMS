using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class WrightsStainTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'WrightsStainTest', 
                        Description: '@TesWri@',
                        EventType : 'editdata', 
                        Mapping: 'wrightsstaintestmapper',
                        StringFields: 'TestResults',
                        Topic : 'Tests', 
                        TableName: 'Tests',
                        Display: [
                            { Label: 'wrightsstainresultId', Translation: '@TesTesA@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ],
                        ValidationRules: [
                            { field: 'wrightsstainresultId', rule: 'required', message: '@GenResA@'}
                        ]
                    }";
        }
    }
}

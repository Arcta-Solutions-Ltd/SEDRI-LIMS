using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class HPyloriantigenTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'HPyloriantigenTest', 
                        Description: '@TesHpy@',
                        EventType : 'editdata', 
                        Mapping: 'hpyloriantigentestmapper',
                        StringFields: 'TestResults',
                        Topic : 'Tests', 
                        TableName: 'Tests',
                        Display: [
                            { Label: 'antResultId', Translation: '@TesTesA@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ],
                        ValidationRules: [
                            { field: 'antResultId', rule: 'required', message: '@GenResA@'}
                        ]
                    }";
        }
    }
}

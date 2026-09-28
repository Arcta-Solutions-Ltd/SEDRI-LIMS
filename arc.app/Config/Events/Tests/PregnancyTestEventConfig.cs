using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class PregnancyTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'PregnancyTest', 
                        Description: '@TesPre@',
                        EventType : 'editdata', 
                        Mapping: 'pregnancytestmapper',
                        StringFields: 'TestResults',
                        Topic : 'Tests', 
                        TableName: 'Tests',
                        Display: [
                            { Label: 'pregnancyId', Translation: '@TesTesA@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ],
                        ValidationRules: [
                            { field: 'pregnancyId', rule: 'required', message: '@GenResA@'}
                        ]
                    }";
        }
    }
}

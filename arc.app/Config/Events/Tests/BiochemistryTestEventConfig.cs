using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class BiochemistryTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'BiochemistryTest', 
                        Description: '@TesBio@',
                        EventType : 'editdata', 
                        Mapping: 'biochemistrytestmapper',
                        StringFields: 'TestResults',
                        Topic : 'Tests', 
                        TableName: 'Tests',
                        Display: [
                            { Label: 'glucose', Translation: '@TesGlu@', List: 'No' },
                            { Label: 'protein', Translation: '@TesPro@', List: 'No' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ],
                        ValidationRules: [
                            { field: 'glucose', rule: 'required', message: '@TesGluB@'},
                            { field: 'protein', rule: 'required', message: '@TesProB@'}
                        ]
                    }";
        }
    }
}

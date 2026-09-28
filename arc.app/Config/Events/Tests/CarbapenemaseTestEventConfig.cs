using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class CarbapenemaseTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'CarbapenemaseTest', 
                        Description: '@TesCarB@',
                        EventType : 'editdata', 
                        Mapping: 'carbapenemasetestmapper',
                        StringFields: 'TestResults',
                        Topic : 'CultureTests', 
                        TableName: 'CultureTests',
                        Display: [
                            { Label: 'carbapenemaseresultId', Translation: '@TesTesA@', List: 'Yes' },
                            { Label: 'resultId', Translation: '@TesTesA@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ],
                        ValidationRules: [
                            { field: 'carbapenemaseresultId', rule: 'required', message: '@GenResA@'}
                        ]
                    }";
        }
    }
}

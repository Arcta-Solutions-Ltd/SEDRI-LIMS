using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class OxidaseTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'OxidaseTest', 
                        Description: '@TesOxi@',
                        EventType : 'editdata', 
                        Mapping: 'oxidasetestmapper',
                        StringFields: 'TestResults',
                        Topic : 'CultureTests', 
                        TableName: 'CultureTests',
                        Display: [
                            { Label: 'oxidaseresultId', Translation: '@TesTesA@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ],
                        ValidationRules: [
                            { field: 'oxidaseresultId', rule: 'required', message: '@GenResA@'}
                        ]
                    }";
        }
    }
}

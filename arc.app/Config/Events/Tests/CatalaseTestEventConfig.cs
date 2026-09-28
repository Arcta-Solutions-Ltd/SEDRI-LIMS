using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class CatalaseTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'CatalaseTest', 
                        Description: '@TesCat@',
                        EventType : 'editdata', 
                        Mapping: 'catalasetestmapper',
                        StringFields: 'TestResults',
                        Topic : 'CultureTests', 
                        TableName: 'CultureTests',
                        Display: [
                            { Label: 'catalaseresultId', Translation: '@TesTesA@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ],
                        ValidationRules: [
                            { field: 'catalaseresultId', rule: 'required', message: '@GenResA@'}
                        ]
                    }";
        }
    }
}

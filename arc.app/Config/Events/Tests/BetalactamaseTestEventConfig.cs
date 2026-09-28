using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class BetalactamaseTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'BetalactamaseTest', 
                        Description: '@TesBet@',
                        EventType : 'editdata', 
                        Mapping: 'betalactamasetestmapper',
                        StringFields: 'TestResults',
                        Topic : 'CultureTests', 
                        TableName: 'CultureTests',
                        Display: [
                            { Label: 'betalactamaseresultid', Translation: '@TesTesA@', List: 'Yes' },
                            { Label: 'resultid', Translation: '@TesTesA@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ],
                        ValidationRules: [
                            { field: 'betalactamaseresultId', rule: 'required', message: '@GenResA@'}
                        ]
                    }";
        }
    }
}

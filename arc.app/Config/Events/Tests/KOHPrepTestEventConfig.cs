using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class KOHPrepTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'KOHPrepTest', 
                        Description: '@TesFun@',
                        EventType : 'editdata', 
                        Mapping: 'kohpreptestmapper',
                        StringFields: 'TestResults',
                        Topic : 'Tests', 
                        TableName: 'Tests',
                        Display: [
                            { Label: 'kohResultId', Translation: '@TesTesA@', List: 'Yes' },
                            { Label: 'kohFungalId', Translation: '@GenFun@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ],
                        ValidationRules: [
                            { field: 'kohresultId', rule: 'required', message: '@GenResA@'}
                        ]
                    }";
        }
    }
}



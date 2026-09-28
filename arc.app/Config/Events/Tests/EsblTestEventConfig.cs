using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EsblTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'EsblTest', 
                        Description: '@TesEsb@',
                        EventType : 'editdata', 
                        Mapping: 'esbltestmapper',
                        StringFields: 'TestResults',
                        Topic : 'CultureTests', 
                        TableName: 'CultureTests',
                        Display: [
                            { Label: 'esblresultId', Translation: '@TesTesA@', List: 'Yes' },
                            { Label: 'resultId', Translation: '@TesTesA@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ],
                        ValidationRules: [
                            { field: 'esblresultId', rule: 'required', message: '@GenResA@'}
                        ]
                    }";
        }
    }
}

using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class APIPanelTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'ApiPanelTest', 
                        Description: '@TesApi@',
                        EventType : 'editdata', 
                        Mapping: 'apipaneltestmapper',
                        StringFields: 'TestResults',
                        Topic : 'CultureTests', 
                        TableName: 'CultureTests',
                        Display: [
                            { Label: 'APIIDPanel', Translation: '@SpeApi@', List: 'Yes' },
                            { Label: 'IdProfile', Translation: '@SpeId@', List: 'No' },
                            { Label: 'PercentageID', Translation: '@SpeIdA@', List: 'No' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ],
                        ValidationRules: [
                            { field: 'APIIDPanel', rule: 'required', message: '@TesApiA@'},
                            { field: 'IdProfile', rule: 'required', message: '@TesIdp@'},
                            { field: 'PercentageID', rule: 'required', message: '@TesPer@'}
                        ]
                    }";
        }
    }
}

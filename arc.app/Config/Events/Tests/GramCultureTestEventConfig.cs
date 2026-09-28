using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class GramCultureTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'GramCultureTest', 
                        Description: '@TesGraA@',
                        EventType : 'editdata', 
                        Mapping: 'gramculturetestmapper',
                        StringFields: 'TestResults',
                        Topic : 'CultureTests', 
                        TableName: 'CultureTests',
                        Display: [
                            { Label: 'gcepicells', Translation: '@TesEpi@', List: 'Yes' },
                            { Label: 'gcorganismgrid', Translation: '@GenOrgB@', List: 'No' },
                            { Label: 'gcorganism', Translation: '@GenOrgA@', List: 'Yes' },
                            { Label: 'gcwbclist', Translation: '@GenSee@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ],
                        ValidationRules: [
                            { field: 'gcepicells', rule: 'required', message: '@TesEpiB@'}
                        ]
                    }";
        }
    }
}

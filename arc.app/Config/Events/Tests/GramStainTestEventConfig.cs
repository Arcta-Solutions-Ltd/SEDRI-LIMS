using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class GramStainTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'GramStainTest', 
                        Description: '@TesGra@',
                        EventType : 'editdata', 
                        Mapping: 'gramstaintestmapper',
                        StringFields: 'TestResults',
                        Topic : 'Tests', 
                        TableName: 'Tests',
                        Display: [
                            { Label: 'wbc', Translation: '@TesWbcB@', List: 'Yes' },
                            { Label: 'epicells', Translation: '@TesEpi@', List: 'Yes' },
                            { Label: 'organismgrid', Translation: '@GenOrgB@', List: 'No' },
                            { Label: 'organism', Translation: '@GenOrgA@', List: 'Yes' },
                            { Label: 'wbclist', Translation: '@GenSee@', List: 'Yes' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ]
                    }";
        }
    }
}

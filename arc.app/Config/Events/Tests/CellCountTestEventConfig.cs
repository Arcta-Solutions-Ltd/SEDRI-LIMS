using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class CellCountTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'CellCountTest', 
                        Description: '@TesCel@',
                        EventType : 'editdata', 
                        Mapping: 'cellcounttestmapper',
                        StringFields: 'TestResults',
                        Topic : 'Tests', 
                        TableName: 'Tests',
                        Display: [
                            { Label: 'ccwbc', Translation: '@TesWbc@', List: 'No' },
                            { Label: 'ccrbc', Translation: '@TesRbc@', List: 'No' },
                            { Label: 'wbcqualitative', Translation: '@TesWbcA@', List: 'Yes' },
                            { Label: 'rbcqualitative', Translation: '@TesRbcA@', List: 'Yes' },
                            { Label: 'polymorphonuclear', Translation: '@TesPol@', List: 'No' },
                            { Label: 'mononuclear', Translation: '@TesMon@', List: 'No' },
                            { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                        ]
                    }";
        }
    }
}

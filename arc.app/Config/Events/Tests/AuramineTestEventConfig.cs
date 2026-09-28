using arc.app.Common;

namespace arc.app.Config.Events;

internal class AuramineTestEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
                    EventName: 'AuramineTest',
                    Description: '@TesAur@',
                    EventType : 'editdata',
                    Mapping: 'auraminetestmapper',
                    StringFields: 'TestResults',
                    Topic : 'Tests',
                    TableName: 'Tests',
                    Display: [
                        { Label: 'auramineId', Translation: '@TesTesA@', List: 'Yes' },
                        { Label: 'printonreport', Translation: '@GenDis@', List: 'No' }
                    ],
                    ValidationRules: [
                        { field: 'auramineId', rule: 'required', message: '@GenResA@'}
                    ]
                }";
    }
}

using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class RemoveDirectTestEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                        EventName: 'removedirecttest',
                        Description: '@ConDelB@',
                        EventType : 'deletedata',
                        Topic : 'Tests',
                        TableName: 'Tests',
                        ValidationRules: [
                            { field: 'Id', rule: 'required', message: '@SpeAte@'}
                        ],
                        Display: [
                            { Label: 'Name', Translation: '@GenTesC@', List: 'No'}
                        ]
                    }";
        }
    }
}

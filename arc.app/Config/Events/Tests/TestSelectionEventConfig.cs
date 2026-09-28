using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class TestSelectionEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'TestSelection', 
                        Description: '@TesUpd@',
                        EventType : 'special', 
                        TableName : 'Tests',
                        Topic: 'Tests',
                        Display: [
                            { Label: 'key', Translation: '@GenKey@', List: 'No' },
                            { Label: 'name', Translation: '@GenNam@', List: 'No' },
                            { Label: 'allowed', Translation: '@GenEnaA@', List: 'No' }
                        ]
                    }";
        }
    }
}

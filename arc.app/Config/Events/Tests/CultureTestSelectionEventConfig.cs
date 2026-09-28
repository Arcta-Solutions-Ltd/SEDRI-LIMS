using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class CultureTestSelectionEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'CultureTestSelection', 
                        Description: '@TesUpdA@',
                        EventType : 'special', 
                        TableName : 'CultureTests',
                        Topic: 'CultureTests',
                        Display: [
                            { Label: 'key', Translation: '@GenKey@', List: 'No' },
                            { Label: 'name', Translation: '@GenNam@', List: 'No' },
                            { Label: 'allowed', Translation: '@GenEnaA@', List: 'No' }
                        ]
                    }";
        }
    }
}

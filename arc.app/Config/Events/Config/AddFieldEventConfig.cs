using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddFieldEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addField',
                        Description: '@ConAddI@',
                        EventType : 'special',
                        TableName: 'configs',
                        Topic : 'Configuration',
                        ValidationRules: [
                            { field: 'Label', rule: 'required', message: '@ConAla@' },
                            { field: 'TypeId', rule: 'required', message: '@ConAty@' }
                        ]
                    }";
        }
    }
}

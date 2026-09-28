using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditFieldEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editField',
                        Description: '@ConEdiH@',
                        EventType : 'special',
                        Topic : 'Configuration',
                        ValidationRules: [
                            { field: 'Label', rule: 'required', message: '@ConAla@' },
                            { field: 'TypeId', rule: 'required', message: '@ConAty@' }
                        ]
                    }";
        }
    }
}

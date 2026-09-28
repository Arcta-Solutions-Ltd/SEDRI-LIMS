using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class EditPageEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'editPage',
                        Description: '@ConEdiX@',
                        EventType : 'special',
                        Topic : 'Configuration',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@ConApa@'},
                            { field: 'Description', rule: 'required', message: '@ConAde@'},
                       ]
                    }";
        }
    }
}

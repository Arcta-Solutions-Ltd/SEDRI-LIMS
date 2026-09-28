using arc.app.Common;

namespace arc.app.Config.Events
{
    internal class AddPageEventConfig : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        EventName: 'addPage',
                        Description: '@ConAddX@',
                        EventType : 'special',
                        TableName: 'configs',
                        Topic : 'Configuration',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@ConApa@'},
                            { field: 'Description', rule: 'required', message: '@ConAde@'},
                       ]
                    }";
        }
    }
}

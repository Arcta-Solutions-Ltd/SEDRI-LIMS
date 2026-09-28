using arc.app.Common;

namespace arc.app.Config.Events.Config;
internal class DeleteMappingEventConfig : IDefinition
{
    public string Get()
    {
        return @"{ 
                        EventName: 'deletemappingevent', 
                        Description: '@MapDelB@',
                        EventType : 'special',
                        Topic : 'Configuration',
                        DataRules: [
                            { type: 'norecord', query: 'deletemappingvalidationquery', message: '@MapDelErr@' }
                        ]
                    }";
    }
}

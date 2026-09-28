using arc.app.Common;

namespace arc.app.Config.Events.Config;
internal class AddMappingEventConfig : IDefinition
{
    public string Get()
    {
        return @"{ 
                        EventName: 'addmappingevent', 
                        Description: '@MapAddB@',
                        EventType : 'specialadddata',
                        Topic : 'Configuration',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@MapAddErr@' }
                        ]
                    }";
    }
}

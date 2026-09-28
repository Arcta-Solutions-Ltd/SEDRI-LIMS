using arc.app.Common;

namespace arc.app.Config.Events.Config;
internal class EditMappingEventConfig : IDefinition
{
    public string Get()
    {
        return @"{ 
                        EventName: 'editmappingevent', 
                        Description: '@MapEdiB@',
                        EventType : 'special',
                        Topic : 'Configuration',
                        ValidationRules: [
                            { field: 'Name', rule: 'required', message: '@MapAddErr@' }
                        ]
                    }";
    }
}

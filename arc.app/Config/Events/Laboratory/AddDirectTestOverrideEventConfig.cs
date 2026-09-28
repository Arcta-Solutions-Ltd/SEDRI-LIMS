using arc.app.Common;

namespace arc.app.Config.Events;

internal class AddDirectTestOverrideEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            EventName: 'adddirecttestoverride',
            Description: '@GenTATG@',
            EventType: 'special',
            Topic: 'Laboratory',
            DoNotSaveInQueue: true
        }";
    }
}

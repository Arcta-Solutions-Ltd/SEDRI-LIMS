using arc.app.Common;

namespace arc.app.Config.Events;

internal class AddCultureTestOverrideEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            EventName: 'addculturetestoverride',
            Description: '@GenTATI@',
            EventType: 'special',
            Topic: 'Laboratory',
            DoNotSaveInQueue: true
        }";
    }
}

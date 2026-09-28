using arc.app.Common;

namespace arc.app.Config.Events;

internal class EditCultureTestOverrideEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            EventName: 'editculturetestoverride',
            Description: '@GenTATK@',
            EventType: 'special',
            Topic: 'Laboratory',
            DoNotSaveInQueue: true
        }";
    }
}

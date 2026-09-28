using arc.app.Common;

namespace arc.app.Config.Events;

internal class EditDirectTestOverrideEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            EventName: 'editdirecttestoverride',
            Description: '@GenTATJ@',
            EventType: 'special',
            Topic: 'Laboratory',
            DoNotSaveInQueue: true
        }";
    }
}

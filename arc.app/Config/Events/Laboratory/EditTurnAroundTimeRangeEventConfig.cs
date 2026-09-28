using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Event configuration for editturnaroundtimerange. Used with deferSave subform; no backend persistence.
/// </summary>
internal class EditTurnAroundTimeRangeEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            EventName: 'editturnaroundtimerange',
            Description: '@GenTATD@',
            EventType: 'special',
            Topic: 'Laboratory',
            DoNotSaveInQueue: true
        }";
    }
}

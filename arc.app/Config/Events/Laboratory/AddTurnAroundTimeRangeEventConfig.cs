using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Event configuration for addturnaroundtimerange. Used with deferSave subform; no backend persistence.
/// </summary>
internal class AddTurnAroundTimeRangeEventConfig : IDefinition
{
    public string Get()
    {
        return @"{
            EventName: 'addturnaroundtimerange',
            Description: '@GenTATB@',
            EventType: 'special',
            Topic: 'Laboratory',
            DoNotSaveInQueue: true
        }";
    }
}

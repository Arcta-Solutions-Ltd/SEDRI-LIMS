using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Update Turn Around Time Config" event.
/// </summary>
internal class UpdateTurnAroundTimeConfigEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the event configuration.
    /// </summary>
    public string Get()
    {
        return @"{
                        EventName: 'updateturnaroundtimeconfig',
                        Description: '@GenTAT@',
                        EventType: 'special',
                        TableName: 'laboratoryconfigs',
                        Topic: 'Laboratory'
                    }";
    }
}

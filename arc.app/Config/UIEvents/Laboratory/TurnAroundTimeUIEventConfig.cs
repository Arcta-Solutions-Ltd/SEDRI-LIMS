using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Turn Around Time" UI event.
/// </summary>
internal class TurnAroundTimeUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Turn Around Time" UI event.
    /// </summary>
    public string Get()
    {
        var newEvent = @"{
                        name: 'turnaroundtimeuievent',
                        description: 'Turn Around Time',
                        type: 'form',
                        action: 'turnaroundtimeform'
                    }";

        return newEvent;
    }
}

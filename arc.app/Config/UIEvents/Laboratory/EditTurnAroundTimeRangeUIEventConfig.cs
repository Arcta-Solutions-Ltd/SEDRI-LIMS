using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Edit Turn Around Time Range" UI event.
/// </summary>
internal class EditTurnAroundTimeRangeUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration.
    /// </summary>
    public string Get()
    {
        return @"{
                        name: 'editturnaroundtimerangeuievent',
                        description: 'Edit Turn Around Time Range',
                        type: 'form',
                        action: 'editturnaroundtimerangeform'
                    }";
    }
}

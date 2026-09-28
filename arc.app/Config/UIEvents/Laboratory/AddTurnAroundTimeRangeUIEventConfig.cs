using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration for the "Add Turn Around Time Range" UI event.
/// </summary>
internal class AddTurnAroundTimeRangeUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration.
    /// </summary>
    public string Get()
    {
        return @"{
                        name: 'addturnaroundtimerangeuievent',
                        description: 'Add Turn Around Time Range',
                        type: 'form',
                        action: 'addturnaroundtimerangeform'
                    }";
    }
}

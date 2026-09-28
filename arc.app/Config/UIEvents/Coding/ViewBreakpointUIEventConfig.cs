using arc.app.Common;

namespace arc.app.Config.UIEvents.Coding;

/// <summary>
/// Defines the UI event configuration used for viewing breakpoint details.
/// </summary>
internal class ViewBreakpointUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the JSON configuration for the 'view breakpoint' UI event.
    /// The configuration specifies the event name, description, type, and action.
    /// </summary>
    /// <returns>A JSON string defining the UI event for viewing breakpoint details.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'viewbreakpointuievent',
                        description: 'View breakpoint details',
                        type: 'view-record',
                        action: 'breakpoints'
                    }";

        return newEvent;
    }
}

using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Provides the definition for the 'report configuration UI' event. 
/// This configuration specifies the visual behavior for viewing report configurations.
/// </summary>
internal class ReportConfigUIEventConfig : IDefinition
{
    /// <summary>
    /// Gets the JSON configuration for the report configuration UI event.
    /// The returned JSON contains details such as the event name, description, type, and action.
    /// </summary>
    /// <returns>A JSON string that defines the report configuration UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'reportconfiguievent',
                        description: 'View report config',
                        type: 'reportdesigner',
                        action: 'reportlistconfig'
                    }";

        return newEvent;
    }
}

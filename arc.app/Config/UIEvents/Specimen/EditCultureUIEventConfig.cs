using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Defines the configuration for the 'editcultureuievent' UI event.
/// </summary>
internal class EditCultureUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string representing the UI event configuration for editing culture.
    /// </summary>
    /// <returns>A JSON string containing event metadata.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'editcultureuievent',
                        description: 'Edit culture',
                        type: 'form',
                        action: 'editcultureform'
                    }";

        return newEvent;
    }
}

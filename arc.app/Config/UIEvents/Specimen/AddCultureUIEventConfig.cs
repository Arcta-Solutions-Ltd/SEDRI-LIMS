using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Provides configuration for the 'addcultureuievent' UI event.
/// </summary>
internal class AddCultureUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the UI event definition as a JSON string.
    /// </summary>
    /// <returns>A JSON-formatted string defining the UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addcultureuievent',
                        description: 'Add culture',
                        type: 'form',
                        action: 'addcultureform'
                    }";

        return newEvent;
    }
}

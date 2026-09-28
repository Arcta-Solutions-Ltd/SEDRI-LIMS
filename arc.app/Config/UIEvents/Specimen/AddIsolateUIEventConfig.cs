using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Provides configuration for the 'addisolateuievent' UI event.
/// </summary>
internal class AddIsolateUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the UI event definition as a JSON string.
    /// </summary>
    /// <returns>A JSON-formatted string defining the UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addisolateuievent',
                        description: 'Add isolate',
                        type: 'form',
                        action: 'addisolateform'
                    }";

        return newEvent;
    }
}

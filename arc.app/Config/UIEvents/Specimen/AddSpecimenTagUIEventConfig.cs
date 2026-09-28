using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Provides configuration for the 'addspecimentaguievent' UI event.
/// </summary>
internal class AddSpecimenTagUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the UI event definition as a JSON string.
    /// </summary>
    /// <returns>A JSON-formatted string defining the UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addspecimentaguievent',
                        description: 'Add Tag',
                        type: 'form',
                        action: 'addspecimentagform'
                    }";

        return newEvent;
    }
}

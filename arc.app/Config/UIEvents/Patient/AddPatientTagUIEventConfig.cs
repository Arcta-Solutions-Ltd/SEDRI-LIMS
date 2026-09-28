using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Provides configuration for the 'addpatienttaguievent' UI event.
/// </summary>
internal class AddPatientTagUIEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the UI event definition as a JSON string.
    /// </summary>
    /// <returns>A JSON-formatted string defining the UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addpatienttaguievent',
                        description: 'Add Tag',
                        type: 'form',
                        action: 'addpatienttagform'
                    }";

        return newEvent;
    }
}

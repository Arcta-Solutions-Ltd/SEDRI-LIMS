using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Represents the configuration for the Add Alert UI event.
/// </summary>
internal class AddAlertUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON string that defines the configuration for the Add Alert UI event.
    /// </summary>
    /// <returns>A JSON string representing the Add Alert UI event configuration.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'addalertuievent',
                        description: 'Add Alert',
                        type: 'form',
                        action: 'addalertform'
                    }";

        return newEvent;
    }
}


using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Configuration class for the accept results UI event.
/// </summary>
internal class AcceptResultsUIEventConfig : IDefinition
{
    /// <summary>
    /// Generates the JSON string for the accept results UI event.
    /// </summary>
    /// <returns>A JSON string that represents the event configuration.</returns>
    public string Get()
    {
        // JSON string representing the event configuration for accepting results from an external instrument.
        var newEvent = @"{
                        name: 'acceptresultsuievent',
                        description: '@InsAccD@',
                        type: 'form',
                        action: 'resultsconfirmationform'
                    }";

        return newEvent;
    }
}

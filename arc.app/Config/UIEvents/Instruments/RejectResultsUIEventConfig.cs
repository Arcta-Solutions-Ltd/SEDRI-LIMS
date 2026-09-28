using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Represents the UI event configuration for rejecting results
/// from an external instrument.
/// </summary>
internal class RejectResultsUIEventConfig : IDefinition
{
    /// <summary>
    /// Builds and returns a JSON string defining the reject results UI event configuration.
    /// </summary>
    /// <returns>A JSON-formatted string describing the UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'rejectresultsuievent',
                        description: '@InsRejC@',
                        type: 'form',
                        action: 'resultsrejectionform'
                    }";

        return newEvent;
    }
}


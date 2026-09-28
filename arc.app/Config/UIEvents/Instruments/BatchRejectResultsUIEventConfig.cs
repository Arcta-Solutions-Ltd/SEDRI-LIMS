using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Represents the UI event configuration for batch rejection of results
/// from an external instrument.
/// </summary>
internal class BatchRejectResultsUIEventConfig : IDefinition
{
    /// <summary>
    /// Builds and returns a JSON string defining the batch reject results UI event configuration.
    /// </summary>
    /// <returns>A JSON-formatted string describing the UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'batchrejectresultsuievent',
                        description: '@InsBatK@',
                        type: 'form',
                        action: 'batchrejectresultsform'
                    }";

        return newEvent;
    }
}


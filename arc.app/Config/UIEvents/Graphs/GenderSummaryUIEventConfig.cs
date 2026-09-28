using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Defines the UI event configuration for the Gender Summary graph.
/// </summary>
internal class GenderSummaryUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration string for the gender summary UI event.
    /// </summary>
    /// <returns>
    /// A JSON-formatted string containing the event name, description, type, and associated action.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'gendersummaryuievent',
                        description: 'Gender Graph',
                        type: 'graph',
                        action: 'gendersummarygraph'
                    }";

        return newEvent;
    }
}

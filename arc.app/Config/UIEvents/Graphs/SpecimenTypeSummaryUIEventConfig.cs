using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Defines the UI event configuration for the Specimen Type Summary graph.
/// </summary>
internal class SpecimenTypeSummaryUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration string for the specimen type summary UI event.
    /// </summary>
    /// <returns>
    /// A JSON-formatted string containing the event name, description, type, and associated action.
    /// </returns>
    public string Get()
    {
        var newEvent = @"{
                        name: 'specimentypesummaryuievent',
                        description: 'Specimen Type Graph',
                        type: 'graph',
                        action: 'specimentypesummarygraph'
                    }";

        return newEvent;
    }
}

using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Provides the configuration for the SpecimenReport UI event.
/// </summary>
internal class SpecimenReportUIEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON representation of the SpecimenReport UI event.
    /// </summary>
    /// <returns>A JSON string defining the UI event.</returns>
    public string Get()
    {
        var newEvent = @"{
            name: 'specimenreportuievent',
            description: 'Specimen Report',
            type: 'form',
            action: 'specimenreportform'
        }";

        return newEvent;
    }
}

using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Provides the configuration for the SpecimenReport special event.
/// </summary>
internal class SpecimenReportEventConfig : IDefinition
{
    /// <summary>
    /// Returns the JSON definition for the SpecimenReport event configuration.
    /// </summary>
    /// <returns>A JSON string defining the SpecimenReport event.</returns>
    public string Get()
    {
        return @"{ 
            EventName: 'specimenreport', 
            Description: '@RepSpe@',
            EventType: 'special', 
            Topic: 'Specimen', 
            TableName: 'Specimen'
        }";
    }
}

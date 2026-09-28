using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Delete Form Specimen Type Option" event.
/// </summary>
internal class DeleteFormSpecimenTypeOptionEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Form Specimen Type Option" event.
    /// </summary>
    /// <returns>
    /// A string representation of the event configuration, detailing the event name, description, type and
    /// the table the record is removed from.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'deleteformspecimentypeoption',
                        Description: '@ConFormSpeDel@',
                        EventType : 'deletedata',
                        Topic : 'Laboratory',
                        TableName: 'LaboratoryConfigs',
                    }";
    }
}

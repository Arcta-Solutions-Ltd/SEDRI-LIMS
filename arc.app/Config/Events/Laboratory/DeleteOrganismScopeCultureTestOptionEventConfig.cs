using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Configuration for the "Delete Organism Scope Culture Test Option" event.
/// </summary>
internal class DeleteOrganismScopeCultureTestOptionEventConfig : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for the "Delete Organism Scope Culture Test Option" event.
    /// </summary>
    /// <returns>
    /// A string representation of the event configuration.
    /// </returns>
    public string Get()
    {
        return @"{
                        EventName: 'deleteOrganismScopeCultureTestOptionEvent',
                        Description: '@LabOrgE@',
                        EventType : 'deletedata',
                        Topic : 'Laboratory',
                        TableName: 'LaboratoryConfigs',
                    }";
    }
}

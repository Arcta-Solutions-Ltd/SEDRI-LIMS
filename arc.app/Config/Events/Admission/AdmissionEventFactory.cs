using arc.app.Common;

namespace arc.app.Config.Events.Admission;

/// <summary>
/// Resolves admission event configuration definitions by name.
/// </summary>
internal class AdmissionEventFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates the admission event definition matching the supplied name.
    /// </summary>
    /// <param name="definitionName">The event definition name.</param>
    /// <returns>The matching definition, or null when this factory does not own the name.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "editadmission" => new EditAdmissionEventConfig(),
            "deleteadmission" => new DeleteAdmissionEventConfig(),
            "viewadmissionrecord" => new ViewAdmissionRecordEventConfig(),
            "manageadmissionattachments" => new ManageAdmissionAttachmentsEventConfig(),
            _ => null,
        };
    }
}

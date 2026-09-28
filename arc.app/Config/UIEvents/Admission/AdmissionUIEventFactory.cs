using arc.app.Common;

namespace arc.app.Config.UIEvents.Admission;

/// <summary>
/// Resolves admission UI event configuration definitions by name.
/// </summary>
internal class AdmissionUIEventFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates the admission UI event definition matching the supplied name.
    /// </summary>
    /// <param name="definitionName">The UI event definition name.</param>
    /// <returns>The matching definition, or null when this factory does not own the name.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "viewadmissionrecorduievent" => new ViewAdmissionRecordUIEventConfig(),
            "editadmissionuievent" => new EditAdmissionUIEventConfig(),
            "deleteadmissionuievent" => new DeleteAdmissionUIEventConfig(),
            "manageadmissionattachmentsuievent" => new ManageAdmissionAttachmentsUIEventConfig(),
            _ => null,
        };
    }
}

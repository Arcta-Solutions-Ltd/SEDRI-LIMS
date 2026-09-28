using arc.app.Common;

namespace arc.app.Config.Forms.Admission;

/// <summary>
/// Resolves admission form configuration definitions by name.
/// </summary>
internal class AdmissionFormFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates the admission form definition matching the supplied name.
    /// </summary>
    /// <param name="definitionName">The form definition name.</param>
    /// <returns>The matching definition, or null when this factory does not own the name.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "editadmissionform" => new EditAdmissionFormConfig(),
            "deleteadmissionform" => new DeleteAdmissionFormConfig(),
            "manageadmissionattachmentsform" => new ManageAdmissionAttachmentsFormConfig(),
            _ => null,
        };
    }
}

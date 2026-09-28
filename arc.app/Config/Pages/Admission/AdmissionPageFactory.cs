using arc.app.Common;

namespace arc.app.Config.Pages.Admission;

/// <summary>
/// Resolves admission page configuration definitions by name.
/// </summary>
internal class AdmissionPageFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates the admission page definition matching the supplied name.
    /// </summary>
    /// <param name="definitionName">The page definition name.</param>
    /// <returns>The matching definition, or null when this factory does not own the name.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "deleteadmissionpage" => new DeleteAdmissionPageConfig(),
            "manageadmissionattachmentspage" => new ManageAdmissionAttachmentsPageConfig(),
            _ => null,
        };
    }
}

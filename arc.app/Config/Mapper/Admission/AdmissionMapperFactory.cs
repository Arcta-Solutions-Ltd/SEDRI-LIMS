using arc.app.Common;

namespace arc.app.Config.Mapper.Admission;

/// <summary>
/// Resolves admission and request mapper definitions by name.
/// </summary>
internal class AdmissionMapperFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates the mapper definition matching the supplied name.
    /// </summary>
    /// <param name="definitionName">The mapper definition name.</param>
    /// <returns>The matching definition, or null when this factory does not own the name.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "admissionviewmapper" => new AdmissionViewMapper(),
            "requestviewmapper" => new RequestViewMapper(),
            "manageadmissionattachmentsmapper" => new ManageAdmissionAttachmentsMapper(),
            "managerequestattachmentsmapper" => new ManageRequestAttachmentsMapper(),
            "admissionattachmentsviewmapper" => new AdmissionAttachmentsViewMapper(),
            "requestattachmentsviewmapper" => new RequestAttachmentsViewMapper(),
            _ => null,
        };
    }
}

using arc.app.Common;

namespace arc.app.Config.Forms.Request;

/// <summary>
/// Resolves request form configuration definitions by name.
/// </summary>
internal class RequestFormFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates the request form definition matching the supplied name.
    /// </summary>
    /// <param name="definitionName">The form definition name.</param>
    /// <returns>The matching definition, or null when this factory does not own the name.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "editrequestform" => new EditRequestFormConfig(),
            "deleterequestform" => new DeleteRequestFormConfig(),
            "managerequestattachmentsform" => new ManageRequestAttachmentsFormConfig(),
            _ => null,
        };
    }
}

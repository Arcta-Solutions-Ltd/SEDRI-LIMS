using arc.app.Common;

namespace arc.app.Config.Pages.Request;

/// <summary>
/// Resolves request page configuration definitions by name.
/// </summary>
internal class RequestPageFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates the request page definition matching the supplied name.
    /// </summary>
    /// <param name="definitionName">The page definition name.</param>
    /// <returns>The matching definition, or null when this factory does not own the name.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "deleterequestpage" => new DeleteRequestPageConfig(),
            "managerequestattachmentspage" => new ManageRequestAttachmentsPageConfig(),
            _ => null,
        };
    }
}

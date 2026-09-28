using arc.app.Common;

namespace arc.app.Config.Pages.Images;

/// <summary>
/// Factory for image page configurations
/// </summary>
internal class ImagePageFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "deleteimagepage" => new DeleteImagePageConfig(),
            "uploadimagepage" => new UploadImagePageConfig(),
            "updateimagepage" => new UpdateImagePageConfig(),
            _ => null,
        };
    }
}

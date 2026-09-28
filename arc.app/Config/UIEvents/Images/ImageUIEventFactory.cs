using arc.app.Common;

namespace arc.app.Config.UIEvents.Images;

/// <summary>
/// Factory for image UI event configurations
/// </summary>
internal class ImageUIEventFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "uploadimageuievent" => new UploadImageUIEventConfig(),
            "updateimageuievent" => new UpdateImageUIEventConfig(),
            "deleteimageuievent" => new DeleteImageUIEventConfig(),
            _ => null,
        };
    }
}

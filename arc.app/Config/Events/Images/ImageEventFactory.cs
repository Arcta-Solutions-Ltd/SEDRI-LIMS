using arc.app.Common;

namespace arc.app.Config.Events.Images;

/// <summary>
/// Factory for image event configurations
/// </summary>
internal class ImageEventFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "uploadimageevent" => new UploadImageEventConfig(),
            "updateimageevent" => new UpdateImageEventConfig(),
            "deleteimageevent" => new DeleteImageEventConfig(),
            _ => null,
        };
    }
}

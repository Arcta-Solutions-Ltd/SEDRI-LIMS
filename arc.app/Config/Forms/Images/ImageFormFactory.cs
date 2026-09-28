using arc.app.Common;

namespace arc.app.Config.Forms.Images;

/// <summary>
/// Factory for image form configurations
/// </summary>
internal class ImageFormFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "deleteimageform" => new DeleteImageFormConfig(),
            "uploadimageform" => new UploadImageFormConfig(),
            "updateimageform" => new UpdateImageFormConfig(),
            _ => null,
        };
    }
}

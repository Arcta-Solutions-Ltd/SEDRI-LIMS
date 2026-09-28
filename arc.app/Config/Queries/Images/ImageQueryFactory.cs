using arc.app.Common;

namespace arc.app.Config.Queries.Images;

/// <summary>
/// Factory for image query configurations
/// </summary>
internal class ImageQueryFactory : IDefinitionFactory
{
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "imagelistquery" => new ImageListQueryConfig(),
            "singleimagequery" => new SingleImageQueryConfig(),
            _ => null,
        };
    }
}

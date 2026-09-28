using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Factory class for creating asset-related mapper definition instances based on the provided definition name.
/// </summary>
internal class AssetMapperFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates a mapper definition instance based on the specified definition name.
    /// </summary>
    /// <param name="definitionName">The name of the mapper definition to create.</param>
    /// <returns>
    /// An instance of the corresponding mapper definition if the name matches; otherwise, null.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "storageparentcountmapper" => new StorageParentCountMapper(),
            _ => null,
        };
    }
}


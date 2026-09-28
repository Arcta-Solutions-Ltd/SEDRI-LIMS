using arc.app.Common;

namespace arc.app.Config.Pages;

/// <summary>
/// Represents a factory for creating page configurations based on definition names.
/// </summary>
internal class AssetPageFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates a page configuration instance based on the given definition name.
    /// </summary>
    /// <param name="definitionName">The name of the definition to create.</param>
    /// <returns>
    /// An instance of a page configuration matching the provided definition name, 
    /// or <c>null</c> if the definition name does not match any known configuration.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addstoragepage" => new AddStoragePageConfig(),
            "deletestoragepage" => new DeleteStoragePageConfig(),
            "editstoragepage" => new EditStoragePageConfig(),
            "addsupplierpage" => new AddSupplierPageConfig(),
            "deletesupplierpage" => new DeleteSupplierPageConfig(),
            "editsupplierpage" => new EditSupplierPageConfig(),
            _ => null,
        };
    }
}


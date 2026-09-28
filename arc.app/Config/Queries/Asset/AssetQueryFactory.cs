using arc.app.Common;

namespace arc.app.Config.Queries.Asset;

/// <summary>
/// Represents a factory for creating query definitions based on definition names.
/// </summary>
internal class AssetQueryFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates a query definition instance based on the given definition name.
    /// </summary>
    /// <param name="definitionName">The name of the definition to create.</param>
    /// <returns>
    /// An instance of a query definition matching the provided definition name, 
    /// or <c>null</c> if the definition name does not match any known query definitions.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "editstoragequery" => new EditStorageQuery(),
            "editsupplierquery" => new EditSupplierQuery(),
            "storagelistquery" => new StorageListQuery(),
            "storageparentcountquery" => new StorageParentCountQuery(),
            "supplierlistquery" => new SupplierListQuery(),
            "singlestorageforstoragelistquery" => new SingleStorageForStorageListQuery(),
            "singlesupplierforsupplierlistquery" => new SingleSupplierForSupplierListQuery(),
            _ => null,
        };
    }
}


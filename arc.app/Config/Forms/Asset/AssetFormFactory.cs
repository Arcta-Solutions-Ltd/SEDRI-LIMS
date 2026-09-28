using arc.app.Common;

namespace arc.app.Config.Forms;

/// <summary>
/// Factory class for creating Asset Form definitions.
/// </summary>
internal class AssetFormFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates and returns the appropriate form definition based on the provided name.
    /// </summary>
    /// <param name="definitionName">The name of the form definition to create.</param>
    /// <returns>An <see cref="IDefinition"/> object corresponding to the specified name, or null if the name is not recognized.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addstorageform" => new AddStorageFormConfig(),
            "deletestorageform" => new DeleteStorageFormConfig(),
            "editstorageform" => new EditStorageFormConfig(),
            "addsupplierform" => new AddSupplierFormConfig(),
            "deletesupplierform" => new DeleteSupplierFormConfig(),
            "editsupplierform" => new EditSupplierFormConfig(),
            _ => null,
        };
    }
}


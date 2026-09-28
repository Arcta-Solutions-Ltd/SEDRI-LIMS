using arc.app.Common;

namespace arc.app.Config.Events;

/// <summary>
/// Factory class for creating Asset Event definitions.
/// </summary>
internal class AssetEventFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates and returns the appropriate event definition based on the provided name.
    /// </summary>
    /// <param name="definitionName">The name of the event definition to create.</param>
    /// <returns>An <see cref="IDefinition"/> object corresponding to the specified name, or null if the name is not recognized.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addstorageevent" => new AddStorageEventConfig(),
            "deletestorageevent" => new DeleteStorageEventConfig(),
            "editstorageevent" => new EditStorageEventConfig(),
            "addsupplierevent" => new AddSupplierEventConfig(),
            "deletesupplierevent" => new DeleteSupplierEventConfig(),
            "editsupplierevent" => new EditSupplierEventConfig(),
            _ => null,
        };
    }
}


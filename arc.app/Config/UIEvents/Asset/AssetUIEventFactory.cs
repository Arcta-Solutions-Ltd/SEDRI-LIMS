using arc.app.Common;

namespace arc.app.Config.UIEvents;

/// <summary>
/// Factory class for creating Asset UI event definitions.
/// </summary>
internal class AssetUIEventFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates and returns the appropriate UI event definition based on the provided name.
    /// </summary>
    /// <param name="definitionName">The name of the UI event definition to create.</param>
    /// <returns>An <see cref="IDefinition"/> object corresponding to the specified name, or null if the name is not recognized.</returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "addstorageuievent" => new AddStorageUIEventConfig(),
            "deletestorageuievent" => new DeleteStorageUIEventConfig(),
            "editstorageuievent" => new EditStorageUIEventConfig(),
            "addsupplieruievent" => new AddSupplierUIEventConfig(),
            "deletesupplieruievent" => new DeleteSupplierUIEventConfig(),
            "editsupplieruievent" => new EditSupplierUIEventConfig(),
            _ => null,
        };
    }
}


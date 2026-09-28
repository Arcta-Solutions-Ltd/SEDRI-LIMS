using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Factory class for creating definition instances based on the provided definition name.
/// </summary>
internal class LocationMapperFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates a definition instance based on the specified definition name.
    /// </summary>
    /// <param name="definitionName">The name of the definition to create.</param>
    /// <returns>
    /// An instance of the corresponding definition if the name matches; otherwise, null.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "locationparentcountmapper" => new LocationParentCountMapper(),
            "locationsuppliercountmapper" => new LocationSupplierCountMapper(),
            _ => null,
        };
    }
}


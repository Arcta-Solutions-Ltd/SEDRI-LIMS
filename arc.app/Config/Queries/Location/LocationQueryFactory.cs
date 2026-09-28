using arc.app.Common;
using arc.app.Config.Queries.Location;

namespace arc.app.Config.Queries;

/// <summary>
/// Factory class for creating location-related query definition instances based on the provided definition name.
/// </summary>
internal class LocationQueryFactory : IDefinitionFactory
{
    /// <summary>
    /// Creates a query definition instance based on the specified definition name.
    /// </summary>
    /// <param name="definitionName">The name of the query definition to create.</param>
    /// <returns>
    /// An instance of the corresponding query definition if the name matches; otherwise, null.
    /// </returns>
    public IDefinition Create(string definitionName)
    {
        return definitionName.ToLower() switch
        {
            "locationbyid" => new LocationByIdQuery(),
            "locationlist" => new LocationListQuery(),
            "locationparentcount" => new LocationParentCountQuery(),
            "locationpatientcount" => new LocationPatientCountQuery(),
            "locationsuppliercountquery" => new LocationSupplierCountQuery(),
            "singlelocationforlocationlist" => new SingleLocationForLocationListQuery(),
            _ => null,
        };
    }
}


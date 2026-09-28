using arc.app.Common;

namespace arc.app.Config.Queries.Specification;

/// <summary>
/// Query configuration for the specificationexists validation (duplicate check).
/// </summary>
internal class SpecificationExistsQuery : IDefinition
{
    public string Get()
    {
        return @"{
                'Query': 'specificationexists', 'TableName': 'Specification', 'Type': 'Special'
            }";
    }
}

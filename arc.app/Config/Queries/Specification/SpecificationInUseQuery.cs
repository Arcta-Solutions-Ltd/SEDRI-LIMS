using arc.app.Common;

namespace arc.app.Config.Queries.Specification;

/// <summary>
/// Query configuration for the specificationinuse validation (prevents deletion when in use).
/// </summary>
internal class SpecificationInUseQuery : IDefinition
{
    public string Get()
    {
        return @"{
                'Query': 'SpecificationInUse', 'TableName': 'Specification', 'Type': 'Special'
            }";
    }
}

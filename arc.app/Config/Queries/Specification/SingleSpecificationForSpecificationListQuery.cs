using arc.app.Common;

namespace arc.app.Config.Queries.Specification;

/// <summary>
/// Query configuration for loading a single specification for the specification list view.
/// </summary>
internal class SingleSpecificationForSpecificationListQuery : IDefinition
{
    public string Get()
    {
        return @"{
                'Query': 'SingleSpecificationForSpecificaitonList', 'TableName': 'Specification', 'Type': 'Special'
            }";
    }
}

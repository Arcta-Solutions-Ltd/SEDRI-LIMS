using arc.app.Common;

namespace arc.app.Config.Queries.Specification;

/// <summary>
/// Query configuration for the specification list view.
/// </summary>
internal class SpecificationListQuery : IDefinition
{
    public string Get()
    {
        return @"{
                'Query': 'SpecificationList', 'TableName': 'Specification', 'Type': 'Special'
            }";
    }
}

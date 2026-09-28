using arc.app.Common;

namespace arc.app.Config.Queries.Specification;

/// <summary>
/// Query configuration for the delete specification flow.
/// </summary>
internal class DeleteSpecificationQuery : IDefinition
{
    public string Get()
    {
        return @"{
                'Query': 'deletespecificationquery', 'TableName': 'Specification', 'Type': 'Special'
            }";
    }
}

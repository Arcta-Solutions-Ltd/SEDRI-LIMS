using arc.app.Common;

namespace arc.app.Config.Queries.Specification;

/// <summary>
/// Query definition for checking if a version number list item is used by any specification.
/// Used when deleting a version to prevent orphaned references.
/// </summary>
internal class CheckWhetherVersionInSpecificationQuery : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                        'Query': 'checkwhetherversioninspecification', 'TableName': 'Specification', 'Type': 'Special',
                        'ParameterMapping': 'idtoversionnumberidmapper'
                    }";
    }
}

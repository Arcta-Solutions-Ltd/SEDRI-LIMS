using arc.app.Common;

namespace arc.app.Config.Queries.Specification;

/// <summary>
/// Query definition for checking if a publication year list item is used by any specification.
/// Used when deleting a publication year to prevent orphaned references.
/// </summary>
internal class CheckWhetherYearInSpecificationQuery : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                        'Query': 'checkwhetheryearinspecification', 'TableName': 'Specification', 'Type': 'Special',
                        'ParameterMapping': 'idtopublicationyearidmapper'
                    }";
    }
}

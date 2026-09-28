using arc.app.Common;

namespace arc.app.Config.Queries.Specification;

/// <summary>
/// Query definition for checking if a document type list item is used by any specification.
/// Used when deleting a document type to prevent orphaned references.
/// </summary>
internal class CheckWhetherDocumentInSpecificationQuery : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                        'Query': 'checkwhetherdocumentinspecification', 'TableName': 'Specification', 'Type': 'Special',
                        'ParameterMapping': 'idtodocumentidmapper'
                    }";
    }
}

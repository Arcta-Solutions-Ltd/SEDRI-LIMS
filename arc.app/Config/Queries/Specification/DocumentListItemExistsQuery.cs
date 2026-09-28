using arc.app.Common;

namespace arc.app.Config.Queries.Specification;

/// <summary>
/// Query definition for checking if a document type list item already exists (duplicate check).
/// </summary>
internal class DocumentListItemExistsQuery : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                        'Query': 'documentlistitemexists', 'TableName': 'ListItem', 'Type': 'Count',
                        'ParameterMapping': 'documentlistexistsmapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'int'}
                        ],
                        'Where' : [
                            {'Field': 'ListId', 'Comparison': 'equals' },
                            {'Field': 'Value', 'Comparison': 'equals' }
                        ]
                    }";
    }
}

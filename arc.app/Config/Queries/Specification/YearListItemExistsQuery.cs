using arc.app.Common;

namespace arc.app.Config.Queries.Specification;

/// <summary>
/// Query definition for checking if a publication year list item already exists (duplicate check).
/// </summary>
internal class YearListItemExistsQuery : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                        'Query': 'yearlistitemexists', 'TableName': 'ListItem', 'Type': 'Count',
                        'ParameterMapping': 'yearlistexistsmapper',
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

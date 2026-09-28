using arc.app.Common;

namespace arc.app.Config.Queries.Coding;

/// <summary>
/// Configuration for the "Tag Has Children" count query.
/// Counts listitemparentchild rows where parentid equals the tag Id.
/// Used by DeleteTagEventConfig DataRule to prevent deleting tags that have child tags.
/// </summary>
internal class TagHasChildrenQuery : IDefinition
{
    /// <summary>
    /// Generates the JSON configuration for the tag has children count query.
    /// </summary>
    public string Get()
    {
        return @"{
                    'Query': 'taghaschildren', 'TableName': 'listitemparentchild', 'Type': 'Count',
                    'ParameterMapping': 'taghaschildrenmapper',
                    'Fields': [
                        {'Name': 'Id', 'Type': 'int'}
                    ],
                    'Where' : [
                        {'Field': 'parentid', 'Comparison': 'equals' }
                    ]
                }";
    }
}

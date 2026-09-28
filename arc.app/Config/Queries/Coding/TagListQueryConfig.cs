using arc.app.Common;

namespace arc.app.Config.Queries.Coding
{
    /// <summary>
    /// Query configuration for the TagList query used by the tags list view.
    /// </summary>
    internal class TagListQueryConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                'Query': 'TagList',
                'TableName': 'ListItem',
                'Type': 'Special'
            }";
        }
    }
}

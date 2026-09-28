using arc.app.Common;

namespace arc.app.Config.Queries.Coding
{
    /// <summary>
    /// Query configuration for the SingleTagForTagList query used by edit/delete tag forms.
    /// </summary>
    internal class SingleTagForTagListQueryConfig : IDefinition
    {
        public string Get()
        {
            return @"{
                'Query': 'SingleTagForTagList',
                'TableName': 'ListItem',
                'Type': 'Special'
            }";
        }
    }
}

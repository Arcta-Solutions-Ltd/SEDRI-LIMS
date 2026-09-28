using arc.app.Common;

namespace arc.app.Config.Queries.Coding;
internal class ExpertRuleListQuery : IDefinition
{
    public string Get()
    {
        return @"{
                'Query': 'expertrulelist', 'TableName': 'ExpertRule', 'Type': 'Special'
            }";
    }
}

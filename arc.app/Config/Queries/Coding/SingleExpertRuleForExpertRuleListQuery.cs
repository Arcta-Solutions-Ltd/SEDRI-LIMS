using arc.app.Common;

namespace arc.app.Config.Queries;
internal class SingleExpertRuleForExpertRuleListQuery : IDefinition
{
    public string Get()
    {
        return @"{
                'Query': 'SingleExpertRuleForExpertRuleList', 'TableName': 'ExpertRule', 'Type': 'Special'
            }";
    }
}

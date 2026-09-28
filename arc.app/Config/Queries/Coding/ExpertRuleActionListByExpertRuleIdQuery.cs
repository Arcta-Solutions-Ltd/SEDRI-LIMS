using arc.app.Common;

namespace arc.app.Config.Queries.Coding;

/// <summary>
/// Query definition for loading expert rule actions for a given expert rule.
/// Used by the expert rule actions list view section on the expert rule record view.
/// </summary>
internal class ExpertRuleActionListByExpertRuleIdQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the expert rule action list query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'ExpertRuleActionListByExpertRuleId',
            'TableName': 'expertruleaction',
            'Type': 'Special',
            'Where': [
                { 'Field': 'ExpertRuleId', 'Comparison': '=' }
            ],
            'Orderby': 'Id'
        }";
    }
}

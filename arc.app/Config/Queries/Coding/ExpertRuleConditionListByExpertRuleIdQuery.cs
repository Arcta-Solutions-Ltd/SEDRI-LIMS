using arc.app.Common;

namespace arc.app.Config.Queries.Coding;

/// <summary>
/// Query definition for loading expert rule conditions for a given expert rule.
/// Used by the expert rule conditions list view section on the expert rule record view.
/// </summary>
internal class ExpertRuleConditionListByExpertRuleIdQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the expert rule condition list query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'ExpertRuleConditionListByExpertRuleId',
            'TableName': 'expertrulecondition',
            'Type': 'Special',
            'Where': [
                { 'Field': 'ExpertRuleId', 'Comparison': '=' }
            ],
            'Orderby': 'Id'
        }";
    }
}

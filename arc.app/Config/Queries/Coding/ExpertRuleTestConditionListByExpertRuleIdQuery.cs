using arc.app.Common;

namespace arc.app.Config.Queries.Coding;

/// <summary>
/// Query definition for loading expert rule test conditions for a given expert rule.
/// Used by the expert rule test conditions list view section on the expert rule record view.
/// </summary>
internal class ExpertRuleTestConditionListByExpertRuleIdQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the expert rule test condition list query.
    /// </summary>
    public string Get()
    {
        return @"{
            'Query': 'ExpertRuleTestConditionListByExpertRuleId',
            'TableName': 'expertruletestcondition',
            'Type': 'Special',
            'Where': [
                { 'Field': 'ExpertRuleId', 'Comparison': '=' }
            ],
            'Orderby': 'Id'
        }";
    }
}

using arc.app.Common;
using arc.app.Config.Queries.Coding;

namespace arc.app.Config.Queries.Coding;

/// <summary>
/// Query configuration for loading a single expert rule test condition for edit from the record view.
/// </summary>
internal class EditExpertRuleTestConditionQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON query configuration for loading an expert rule test condition by id.
    /// </summary>
    /// <returns>JSON string defining the special query executed by the expert rule test condition edit query handler.</returns>
    public string Get()
    {
        return @"{  
                        'Query': 'editexpertruletestconditionquery', 'TableName': 'ExpertRuleTestCondition', 'Type': 'Special'
                    }";
    }
}

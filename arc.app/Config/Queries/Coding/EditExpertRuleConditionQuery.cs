using arc.app.Common;

namespace arc.app.Config.Queries.Coding;

/// <summary>
/// Query configuration for loading a single expert rule condition for edit from the record view.
/// </summary>
internal class EditExpertRuleConditionQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON query configuration for loading an expert rule condition by id.
    /// </summary>
    /// <returns>JSON string defining the special query executed by the expert rule condition edit query handler.</returns>
    public string Get()
    {
        return @"{  
                        'Query': 'editexpertruleconditionquery', 'TableName': 'ExpertRuleCondition', 'Type': 'Special'
                    }";
    }
}

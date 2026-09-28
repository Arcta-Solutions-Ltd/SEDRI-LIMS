using arc.app.Common;

namespace arc.app.Config.Queries.Coding;

/// <summary>
/// Query configuration for loading a single expert rule action for edit from the record view.
/// </summary>
internal class EditExpertRuleActionQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON query configuration for loading an expert rule action by id.
    /// </summary>
    /// <returns>JSON string defining the single-record query and result mapping.</returns>
    public string Get()
    {
        return @"{  
                        'Query': 'editexpertruleactionquery', 'TableName': 'ExpertRuleAction', 'Type': 'Special'
                    }";
    }
}

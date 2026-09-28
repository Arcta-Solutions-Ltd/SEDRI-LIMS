using arc.app.Common;

namespace arc.app.Config.Queries.Coding;

/// <summary>
/// Query configuration for loading an expert rule test condition for delete confirmation.
/// </summary>
internal class DeleteExpertRuleTestConditionQuery : IDefinition
{
    /// <summary>
    /// Returns the JSON query configuration for the delete expert rule test condition form.
    /// </summary>
    /// <returns>JSON string defining the single-record query and result mapping.</returns>
    public string Get()
    {
        return @"{ 
                        'Query': 'deleteexpertruletestconditionquery', 
                        'TableName': 'ExpertRuleTestCondition', 
                        'Type': 'Single',
                        'ResultMapping': 'deleteexpertruletestconditionquerymapper',
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int' },
                            { 'Name': 'TestName', 'Type': 'string' },
                            { 'Name': 'FieldName', 'Type': 'string' },
                            { 'Name': 'Comparison', 'Type': 'string' },
                            { 'Name': 'CompValue', 'Type': 'string' }
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' }
                        ]
                    }";
    }
}

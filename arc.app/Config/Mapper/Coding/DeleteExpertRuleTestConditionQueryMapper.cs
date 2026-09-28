using arc.app.Common;

namespace arc.app.Config.Mapper.Coding;

/// <summary>
/// Maps expert rule test condition query results to delete confirmation form fields.
/// </summary>
internal class DeleteExpertRuleTestConditionQueryMapper : IDefinition
{
    /// <summary>
    /// Returns the mapper configuration for delete expert rule test condition initial query results.
    /// </summary>
    /// <returns>JSON mapper definition.</returns>
    public string Get()
    {
        return @"{  
                        'Name': 'deleteexpertruletestconditionquerymapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'TestName', Value: 'TestName' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'FieldName', Value: 'FieldName' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'Comparison', Value: 'Comparison' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'CompValue', Value: 'CompValue' }
                        ],
                        'Target': 
                            {
                                'TestName':'<:1:>',
                                'FieldName':'<:2:>',
                                'Comparison':'<:3:>',
                                'CompValue':'<:4:>'
                            }
                     }";
    }
}

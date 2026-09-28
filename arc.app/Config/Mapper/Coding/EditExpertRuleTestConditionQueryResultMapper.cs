using arc.app.Common;

namespace arc.app.Config.Mapper.Coding;

/// <summary>
/// Maps expert rule test condition query results to edit form field ids.
/// </summary>
internal class EditExpertRuleTestConditionQueryResultMapper : IDefinition
{
    /// <summary>
    /// Returns the mapper configuration for edit expert rule test condition initial query results.
    /// </summary>
    /// <returns>JSON mapper definition.</returns>
    public string Get()
    {
        return @"{  
                        'Name': 'editexpertruletestconditionqueryresultmapper',
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'TestName', Value: 'TestConditionName' }
                        ],
                        'Target' : { 
                            Id: '<:1:>',
                            TestConditionName: '<:2:>'
                        }
                     }";
    }
}

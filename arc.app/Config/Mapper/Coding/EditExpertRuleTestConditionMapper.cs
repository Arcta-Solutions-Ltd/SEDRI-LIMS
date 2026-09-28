using arc.app.Common;

namespace arc.app.Config.Mapper.Coding;

/// <summary>
/// Maps edit expert rule test condition form data to the ExpertRuleTestCondition table.
/// </summary>
internal class EditExpertRuleTestConditionMapper : IDefinition
{
    /// <summary>
    /// Returns the mapper configuration for editing an expert rule test condition.
    /// </summary>
    /// <returns>JSON mapper definition.</returns>
    public string Get()
    {
        return @"{  
                        'Name': 'editexpertruletestconditionmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'TestConditionName', Value: 'TestConditionName' }
                        ],
                        'Target' : {
                            Id: '<:1:>',
                            TestName: '<:2:>'
                        }
                     }";
    }
}

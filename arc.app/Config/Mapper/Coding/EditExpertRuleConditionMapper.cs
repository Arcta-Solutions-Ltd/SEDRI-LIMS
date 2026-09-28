using arc.app.Common;

namespace arc.app.Config.Mapper.Coding;

/// <summary>
/// Maps edit expert rule condition form data to the ExpertRuleCondition table.
/// </summary>
internal class EditExpertRuleConditionMapper : IDefinition
{
    /// <summary>
    /// Returns the mapper configuration for editing an expert rule condition.
    /// </summary>
    /// <returns>JSON mapper definition.</returns>
    public string Get()
    {
        return @"{  
                        'Name': 'editexpertruleconditionmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'antibioticid', Value: 'antibioticid' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'antibioticgroupid', Value: 'antibioticgroupid' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'SusceptibilityId', Value: 'SusceptibilityId' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'testmethodid', Value: 'testmethodid' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'SpecialConsiderationId', Value: 'SpecialConsiderationId' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'startval', Value: 'startval' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'endval', Value: 'endval' }
                        ],
                        'Target' : {
                            Id: '<:1:>',
                            AntibioticId: '<:2:>',
                            AntibioticGroupId: '<:3:>',
                            SusceptibilityId: '<:4:>',
                            TestMethodId: '<:5:>',
                            SpecialConsiderationId: '<:6:>',
                            StartVal: '<:7:>',
                            EndVal: '<:8:>'
                        }
                     }";
    }
}

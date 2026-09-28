using arc.app.Common;

namespace arc.app.Config.Mapper.Coding;

/// <summary>
/// Maps expert rule condition query results to edit form field ids.
/// </summary>
internal class EditExpertRuleConditionResultMapper : IDefinition
{
    /// <summary>
    /// Returns the mapper configuration for edit expert rule condition initial query results.
    /// </summary>
    /// <returns>JSON mapper definition.</returns>
    public string Get()
    {
        return @"{  
                        'Name': 'editexpertruleconditionqueryresultmapper',
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'AntibioticId', Value: 'antibioticid' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'AntibioticGroupId', Value: 'antibioticgroupid' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'TestMethodId', Value: 'testmethodid' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'SusceptibilityId', Value: 'SusceptibilityId' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'SpecialConsiderationId', Value: 'SpecialConsiderationId' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'StartVal', Value: 'startval' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'EndVal', Value: 'endval' }
                        ],
                        'Target' : { 
                            Id: '<:1:>',
                            antibioticid: '<:2:>',
                            antibioticgroupid: '<:3:>',
                            testmethodid: '<:4:>',
                            SusceptibilityId: '<:5:>',
                            SpecialConsiderationId: '<:6:>',
                            startval: '<:7:>',
                            endval: '<:8:>'
                        }
                     }";
    }
}

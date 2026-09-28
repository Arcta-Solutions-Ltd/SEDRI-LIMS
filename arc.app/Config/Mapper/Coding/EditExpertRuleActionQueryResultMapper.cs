using arc.app.Common;

namespace arc.app.Config.Mapper.Coding;

/// <summary>
/// Maps expert rule action query results to edit form field ids.
/// </summary>
internal class EditExpertRuleActionQueryResultMapper : IDefinition
{
    /// <summary>
    /// Returns the mapper configuration for edit expert rule action initial query results.
    /// </summary>
    /// <returns>JSON mapper definition.</returns>
    public string Get()
    {
        return @"{  
                        'Name': 'editexpertruleactionqueryresultmapper',
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'AntibioticId', Value: 'antibioticid' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'AntibioticGroupId', Value: 'antibioticgroupid' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'SusceptibilityId', Value: 'SusceptibilityId' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'DisplayOnReport', Value: 'DisplayOnReport' }
                        ],
                        'Target' : { 
                            Id: '<:1:>',
                            antibioticid: '<:2:>',
                            antibioticgroupid: '<:3:>',
                            SusceptibilityId: '<:4:>',
                            DisplayOnReport: '<:5:>'
                        }
                     }";
    }
}

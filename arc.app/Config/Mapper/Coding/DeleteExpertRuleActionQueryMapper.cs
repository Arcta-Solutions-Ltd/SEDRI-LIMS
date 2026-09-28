using arc.app.Common;

namespace arc.app.Config.Mapper.Coding;

/// <summary>
/// Maps expert rule action query results to delete confirmation form fields.
/// </summary>
internal class DeleteExpertRuleActionQueryMapper : IDefinition
{
    /// <summary>
    /// Returns the mapper configuration for delete expert rule action initial query results.
    /// </summary>
    /// <returns>JSON mapper definition.</returns>
    public string Get()
    {
        return @"{  
                        'Name': 'deleteexpertruleactionquerymapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'AntibioticName', Value: 'AntibioticName' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'SusceptibilityName', Value: 'SusceptibilityName' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'DisplayOnReport', Value: 'DisplayOnReport' }
                        ],
                        'Target': 
                            {
                                'AntibioticName':'<:1:>',
                                'SusceptibilityName':'<:2:>',
                                'DisplayOnReport':'<:3:>'
                            }
                     }";
    }
}

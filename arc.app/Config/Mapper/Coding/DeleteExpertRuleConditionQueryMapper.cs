using arc.app.Common;

namespace arc.app.Config.Mapper.Coding;

/// <summary>
/// Maps expert rule condition query results to delete confirmation form fields.
/// </summary>
internal class DeleteExpertRuleConditionQueryMapper : IDefinition
{
    /// <summary>
    /// Returns the mapper configuration for delete expert rule condition initial query results.
    /// </summary>
    /// <returns>JSON mapper definition.</returns>
    public string Get()
    {
        return @"{  
                        'Name': 'deleteexpertruleconditionquerymapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'AntibioticName', Value: 'AntibioticName' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'SusceptibilityName', Value: 'SusceptibilityName' }
                        ],
                        'Target': 
                            {
                                'AntibioticName':'<:1:>',
                                'SusceptibilityName':'<:2:>'
                            }
                     }";
    }
}

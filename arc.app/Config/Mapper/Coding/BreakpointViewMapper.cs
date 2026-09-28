using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Defines the result mapping for the breakpoint view. Maps breakpoint query result fields
/// to the view model used on the breakpoint record/detail screen.
/// </summary>
/// <remarks>
/// Provides a Standard mapper with Rules (source-to-value mappings) and a Target that defines
/// the breakpoint details section and its fields (organism, order, family, antibiotic, test method, source, etc.).
/// </remarks>
internal class BreakpointViewMapper : IDefinition
{
    /// <summary>
    /// Returns the JSON mapping definition for the breakpoint view, including Rules and Target layout.
    /// </summary>
    /// <returns>Mapper definition with field mappings and breakpoint details section configuration.</returns>
    public string Get()
    {
        return @"{  
                        'Name': 'specimenviewmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'OrderName', Value: 'OrderName' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'FamilyName', Value: 'FamilyName' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'OrganismName', Value: 'OrganismName' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'OrgGroupName', Value: 'OrgGroupName' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'AntibioticName', Value: 'AntibioticName' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'TestMethod', Value: 'TestMethod' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'Specification', Value: 'Specification' },
                            { Key: '<:9:>', Type: 'Mapping', Source: 'SpecialConsider', Value: 'SpecialConsider' },
                            { Key: '<:10:>', Type: 'Mapping', Source: 'AntibioticDosage', Value: 'AntibioticDosage' },
                            { Key: '<:11:>', Type: 'Mapping', Source: 'Host', Value: 'Host' },
                            { Key: '<:12:>', Type: 'Mapping', Source: 'Enabled', Value: 'Enabled' }
                        ],
                        'Target': 
                            {
                                Sections: [
                                    {
                                        Id: 'breakpointdetails',
                                        Title: '@BreDet@',
                                        Fields: [
                                            { Id: 'OrganismName', Label: '@GenOrgA@', Value: '<:4:>' },
                                            { Id: 'OrderName', Label: '@GenOrd@', Value: '<:2:>' },
                                            { Id: 'FamilyName', Label: '@GenFam@', Value: '<:3:>' },
                                            { Id: 'OrgGroupName', Label: '@GenOrgE@', Value: '<:5:>' },
                                            { Id: 'AntibioticName', Label: '@GenAnt@', Value: '<:6:>' },
                                            { Id: 'AntibioticDosage', Label: '@GenDos@', Value: '<:10:>' },
                                            { Id: 'TestMethod', Label: '@BreTes@', Value: '<:7:>' },
                                            { Id: 'Specification', Label: '@BreSpf@', Value: '<:8:>' },
                                            { Id: 'SpecialConsider', Label: '@BreSpe@', Value: '<:9:>' },
                                            { Id: 'Host', Label: '@GenHos@', Value: '<:11:>' },
                                            { Id: 'Enabled', Label: '@ManQcEna@', Value: '<:12:>' }
                                        ]
                                    }
                                ]
                            }
                     }";
    }
}



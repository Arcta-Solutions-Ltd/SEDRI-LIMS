using arc.app.Common;

namespace arc.app.Config.Mapper.Coding;

/// <summary>
/// Defines the result mapping for the expert rule view. Maps expert rule query result fields
/// to the view model used on the expert rule record/detail screen.
/// </summary>
/// <remarks>
/// Provides a Standard mapper with Rules (source-to-value mappings) and a Target that defines
/// the expert rule details section and its fields. List fields and multiselect specimen types
/// are displayed as text (comma-separated for multiselect).
/// </remarks>
internal class ExpertRuleViewMapper : IDefinition
{
    /// <summary>
    /// Returns the JSON mapping definition for the expert rule view, including Rules and Target layout.
    /// </summary>
    /// <returns>Mapper definition with field mappings and expert rule details section configuration.</returns>
    public string Get()
    {
        return @"{
            'Name': 'expertruleviewmapper',
            'Type': 'Standard',
            'Rules': [
                { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                { Key: '<:2:>', Type: 'Mapping', Source: 'ExpertRuleName', Value: 'ExpertRuleName' },
                { Key: '<:3:>', Type: 'Mapping', Source: 'RuleText', Value: 'RuleText' },
                { Key: '<:4:>', Type: 'Mapping', Source: 'OrderName', Value: 'OrderName' },
                { Key: '<:5:>', Type: 'Mapping', Source: 'FamilyName', Value: 'FamilyName' },
                { Key: '<:6:>', Type: 'Mapping', Source: 'OrganismName', Value: 'OrganismName' },
                { Key: '<:7:>', Type: 'Mapping', Source: 'OrgGroupName', Value: 'OrgGroupName' },
                { Key: '<:8:>', Type: 'Mapping', Source: 'Specification', Value: 'Specification' },
                { Key: '<:9:>', Type: 'Mapping', Source: 'CombinationRule', Value: 'CombinationRule' },
                { Key: '<:10:>', Type: 'Mapping', Source: 'Enabled', Value: 'Enabled' },
                { Key: '<:11:>', Type: 'Mapping', Source: 'AlertOnRule', Value: 'AlertOnRule' },
                { Key: '<:12:>', Type: 'Mapping', Source: 'TagId', Value: 'TagId' },
                { Key: '<:13:>', Type: 'Mapping', Source: 'SpecimenTypesToInclude', Value: 'SpecimenTypesToInclude' },
                { Key: '<:14:>', Type: 'Mapping', Source: 'SpecimenTypesToExclude', Value: 'SpecimenTypesToExclude' }
            ],
            'Target': {
                Sections: [
                    {
                        Id: 'expertruledetails',
                        Title: '@RulManA@',
                        Fields: [
                            { Id: 'ExpertRuleName', Label: '@GenNam@', Value: '<:2:>' },
                            { Id: 'RuleText', Label: '@GenDes@', Value: '<:3:>', Type: 'multiline' },
                            { Id: 'OrderName', Label: '@GenOrd@', Value: '<:4:>' },
                            { Id: 'FamilyName', Label: '@GenFam@', Value: '<:5:>' },
                            { Id: 'OrganismName', Label: '@GenOrgA@', Value: '<:6:>' },
                            { Id: 'OrgGroupName', Label: '@GenOrgE@', Value: '<:7:>' },
                            { Id: 'Specification', Label: '@BreSpf@', Value: '<:8:>' },
                            { Id: 'CombinationRule', Label: '@GenRulP@', Value: '<:9:>' },
                            { Id: 'Enabled', Label: '@GenEna@', Value: '<:10:>' },
                            { Id: 'AlertOnRule', Label: '@RulAle@', Value: '<:11:>' },
                            { Id: 'TagId', Label: '@GenTag@', Value: '<:12:>' },
                            { Id: 'SpecimenTypesToInclude', Label: '@SpeSelN@', Value: '<:13:>' },
                            { Id: 'SpecimenTypesToExclude', Label: '@SpeSelO@', Value: '<:14:>' }
                        ]
                    }
                ]
            }
        }";
    }
}

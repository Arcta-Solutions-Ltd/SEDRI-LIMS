using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Defines the result mapping for the alert view. Maps alert query result fields
/// to the view model used on the alert record/detail screen.
/// </summary>
internal class AlertViewMapper : IDefinition
{
    public string Get()
    {
        return @"{
            'Name': 'alertviewmapper',
            'Type': 'Standard',
            'Rules': [
                { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                { Key: '<:2:>', Type: 'Mapping', Source: 'AlertName', Value: 'AlertName' },
                { Key: '<:3:>', Type: 'Mapping', Source: 'AlertMessage', Value: 'AlertMessage' },
                { Key: '<:4:>', Type: 'Mapping', Source: 'OrganismName', Value: 'OrganismName' },
                { Key: '<:5:>', Type: 'Mapping', Source: 'OrderName', Value: 'OrderName' },
                { Key: '<:6:>', Type: 'Mapping', Source: 'FamilyName', Value: 'FamilyName' },
                { Key: '<:7:>', Type: 'Mapping', Source: 'OrgGroupName', Value: 'OrgGroupName' },
                { Key: '<:8:>', Type: 'Mapping', Source: 'AlertType', Value: 'AlertType' },
                { Key: '<:9:>', Type: 'Mapping', Source: 'Specification', Value: 'Specification' },
                { Key: '<:10:>', Type: 'Mapping', Source: 'Enabled', Value: 'Enabled' },
                { Key: '<:11:>', Type: 'Mapping', Source: 'SusceptibilityAndOr', Value: 'SusceptibilityAndOr' },
                { Key: '<:12:>', Type: 'Mapping', Source: 'TestAndOr', Value: 'TestAndOr' }
            ],
            'Target': {
                Sections: [
                    {
                        Id: 'alertdetails',
                        Title: '@AleDet@',
                        Fields: [
                            { Id: 'AlertName', Label: '@AleAle@', Value: '<:2:>' },
                            { Id: 'AlertMessage', Label: '@GenMes@', Value: '<:3:>' },
                            { Id: 'OrganismName', Label: '@GenOrgA@', Value: '<:4:>' },
                            { Id: 'OrderName', Label: '@GenOrd@', Value: '<:5:>' },
                            { Id: 'FamilyName', Label: '@GenFam@', Value: '<:6:>' },
                            { Id: 'OrgGroupName', Label: '@GenOrgE@', Value: '<:7:>' },
                            { Id: 'AlertType', Label: '@AleTyp@', Value: '<:8:>' },
                            { Id: 'Specification', Label: '@BreSpf@', Value: '<:9:>' },
                            { Id: 'Enabled', Label: '@GenEna@', Value: '<:10:>' },
                            { Id: 'SusceptibilityAndOr', Label: '@AleSusA@', Value: '<:11:>' },
                            { Id: 'TestAndOr', Label: '@AleTesA@', Value: '<:12:>' }
                        ]
                    }
                ]
            }
        }";
    }
}

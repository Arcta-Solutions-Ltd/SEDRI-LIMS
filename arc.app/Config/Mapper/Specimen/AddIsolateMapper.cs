using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Provides the ‘addisolatemapper’ mapping definition for adding isolate entries.
/// Implements <see cref="IDefinition"/> to supply a JSON configuration string
/// that maps source fields to target specimen attributes.
/// </summary>
internal class AddIsolateMapper : IDefinition
{
    /// <summary>
    /// Returns the JSON configuration for the addisolatemapper:
    /// - Name and Type of the mapper
    /// - A list of mapping rules linking source fields (e.g., ParentCultureId, OrganismId) to internal keys
    /// - A target object binding those keys to domain-specific properties
    /// </summary>
    public string Get()
    {
        return @"{  
                        'Name': 'addisolatemapper',  
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'ParentCultureId', Value: 'ParentCultureId' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'OrganismId', Value: 'OrganismId' },
                            { Key: '<:17:>', Type: 'Mapping', Source: 'OrgGroupCodingId', Value: 'OrgGroupCodingId' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'Quantity', Value: 'Quantity' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'IdPercentage', Value: 'IdPercentage' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'PositiveDate', Value: 'PositiveDate' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'PositiveTime', Value: 'PositiveTime' },
                            { Key: '<:11:>', Type: 'Mapping', Source: 'Comment1', Value: 'Comment1' },
                            { Key: '<:12:>', Type: 'Mapping', Source: 'Comment2', Value: 'Comment2' },
                            { Key: '<:13:>', Type: 'Mapping', Source: 'AdditionalNotes', Value: 'AdditionalNotes' },
                            { Key: '<:14:>', Type: 'Mapping', Source: 'AliquotID', Value: 'AliquotID' },
                            { Key: '<:15:>', Type: 'Mapping', Source: 'DisplayInReport', Value: 'DisplayInReport' },
                            { Key: '<:16:>', Type: 'Mapping', Source: 'CultureType', Value: 'CultureType' },
                            { Key: '<:18:>', Type: 'Mapping', Source: 'ManufacturersBarcode', Value: 'ManufacturersBarcode' },
                            { Key: '<:19:>', Type: 'Mapping', Source: 'View', Value: 'View' },
                            { Key: '<:20:>', Type: 'Mapping', Source: 'CultureBottleWeight', Value: 'CultureBottleWeight' },
                            { Key: '<:21:>', Type: 'Mapping', Source: 'CultureBloodAndBottleWeight', Value: 'CultureBloodAndBottleWeight' },
                            { Key: '<:22:>', Type: 'Mapping', Source: 'GrowthId', Value: 'GrowthId' },
                            { Key: '<:23:>', Type: 'Mapping', Source: 'SpecimenId', Value: 'SpecimenId' }
                        ],
                        'Target':  
                            {
                                'SpecimenId':'<:23:>',
                                'TypeId':'<:16:>',
                                'SpecimenOrganismId':'<:2:>',
                                'OrgGroupCodingId':'<:17:>',
                                'SpecimenQuantityId':'<:3:>',
                                'IdPercentage':'<:6:>',
                                'PositiveDate':'<:7:>',
                                'PositiveTime':'<:8:>',
                                'CommentOneId':'<:11:>',
                                'CommentTwoId':'<:12:>',
                                'AdditionalNotes':'<:13:>',
                                'AloquatId':'<:14:>',
                                'DisplayOnReport':'<:15:>',
                                'ManufacturersBarcode':'<:18:>',
                                'View':'<:19:>',
                                'CultureBottleWeight':'<:20:>',
                                'CultureBloodAndBottleWeight':'<:21:>',
                                'GrowthId':'<:22:>',
                                'ParentCultureId': '<:1:>'
                            }
                 }";
    }
}

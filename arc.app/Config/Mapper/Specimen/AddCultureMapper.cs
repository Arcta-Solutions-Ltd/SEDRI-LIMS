using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Provides a definition for the 'addculturemapper' mapping configuration.
/// Implements <see cref="IDefinition"/> to return a structured JSON string
/// that maps source keys to target specimen attributes.
/// </summary>
internal class AddCultureMapper : IDefinition
{
    /// <summary>
    /// Returns a JSON string containing:
    /// - A list of mapping rules linking source fields to internal keys
    /// - A target object that binds those keys to domain-specific attributes
    /// Used for culture data transformation and integration workflows.
    /// </summary>
    public string Get()
    {
        return @"{  
                        'Name': 'addculturemapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
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
                            { Key: '<:22:>', Type: 'Mapping', Source: 'GrowthId', Value: 'GrowthId' }
                        ],
                        'Target': 
                            {
                                'SpecimenId':'<:1:>',
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
                                'GrowthId':'<:22:>'
                            }
                 }";
    }
}

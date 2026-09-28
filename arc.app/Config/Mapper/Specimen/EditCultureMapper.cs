using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Provides a static mapping definition for culture-related data transformations used in event processing.
/// </summary>
internal class EditCultureMapper : IDefinition
{
    /// <summary>
    /// Returns a JSON-formatted string that defines key-value mapping rules between source culture fields 
    /// and their corresponding target identifiers. Used to standardize data flow in edit culture operations.
    /// </summary>
    /// <returns>
    /// A JSON string containing mapping rules, target field bindings, and metadata for the edit culture workflow.
    /// </returns>
    public string Get()
    {
        return @"{  
                        'Name': 'editculturemapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'CultureType', Value: 'CultureType' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'OrganismId', Value: 'OrganismId' },
                            { Key: '<:17:>', Type: 'Mapping', Source: 'OrgGroupCodingId', Value: 'OrgGroupCodingId' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'Quantity', Value: 'Quantity' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'IdPercentage', Value: 'IdPercentage' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'PositiveDate', Value: 'PositiveDate' },
                            { Key: '<:9:>', Type: 'Mapping', Source: 'PositiveTime', Value: 'PositiveTime' },
                            { Key: '<:10:>', Type: 'Mapping', Source: 'Comment1', Value: 'Comment1' },
                            { Key: '<:11:>', Type: 'Mapping', Source: 'Comment2', Value: 'Comment2' },
                            { Key: '<:12:>', Type: 'Mapping', Source: 'AdditionalNotes', Value: 'AdditionalNotes' },
                            { Key: '<:13:>', Type: 'Mapping', Source: 'AliquotID', Value: 'AliquotID' },
                            { Key: '<:14:>', Type: 'Mapping', Source: 'DisplayInReport', Value: 'DisplayInReport' },
                            { Key: '<:15:>', Type: 'Mapping', Source: 'ManufacturersBarcode', Value: 'ManufacturersBarcode' },
                            { Key: '<:16:>', Type: 'Mapping', Source: 'GrowthId', Value: 'GrowthId' },
                            { Key: '<:18:>', Type: 'Mapping', Source: 'ParentCultureId', Value: 'ParentCultureId' },
                            { Key: '<:19:>', Type: 'Mapping', Source: 'CultureBottleWeight', Value: 'CultureBottleWeight' },
                            { Key: '<:20:>', Type: 'Mapping', Source: 'CultureBloodAndBottleWeight', Value: 'CultureBloodAndBottleWeight' }
                        ],
                        'Target': 
                            {
                                'Id':'<:1:>',
                                'TypeId':'<:2:>',
                                'SpecimenOrganismId':'<:3:>',
                                'OrgGroupCodingId':'<:17:>',
                                'SpecimenQuantityId':'<:4:>',
                                'IdPercentage':'<:7:>',
                                'PositiveDate':'<:8:>',
                                'PositiveTime':'<:9:>',
                                'CommentOneId':'<:10:>',
                                'CommentTwoId':'<:11:>',
                                'AdditionalNotes':'<:12:>',
                                'AloquatId':'<:13:>',
                                'DisplayOnReport':'<:14:>',
                                'ManufacturersBarcode':'<:15:>',
                                'GrowthId':'<:16:>',
                                'ParentCultureId':'<:18:>',
                                'CultureBottleWeight':'<:19:>',
                                'CultureBloodAndBottleWeight':'<:20:>'
                            }
                 }";
    }
}

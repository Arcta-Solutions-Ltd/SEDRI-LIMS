using arc.app.Common;

namespace arc.app.Config.Mapper.Specimen;

/// <summary>
/// Mapper for adding a specimen tag. Maps SpecimenId (from record context) and ListItemId (TagId) to SpecimenTag insert.
/// </summary>
internal class AddSpecimenTagMapper : IDefinition
{
    /// <summary>
    /// Retrieves the mapper configuration.
    /// </summary>
    public string Get()
    {
        return @"{
                        'Name': 'addspecimentagmapper',
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'TagId', Value: 'TagId' }
                        ],
                        'Target': {
                            'SpecimenId': '<:1:>',
                            'ListItemId': '<:2:>'
                        }
                    }";
    }
}

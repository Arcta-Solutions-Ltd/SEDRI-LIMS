using arc.app.Common;

namespace arc.app.Config.Mapper.Specification;

/// <summary>
/// Mapper for adding a publication year list item. Maps form data to ListItem insert.
/// ListId 136 = PublicationYear list (created in migration if not exists).
/// </summary>
internal class AddYearMapper : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                        'Name': 'addyearmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            ListId: 136,
                            Value: '<:1:>',
                            Fixed: false,
                            Enabled: true,
                            Deleted: false,
                            DisplayOrder: 1
                        }
                     }";
    }
}

using arc.app.Common;

namespace arc.app.Config.Mapper.Specification;

/// <summary>
/// Mapper for adding a document type list item. Maps form data to ListItem insert.
/// ListId 135 = DocumentType list (created in migration if not exists).
/// </summary>
internal class AddDocumentMapper : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                        'Name': 'adddocumentmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            ListId: 135,
                            Value: '<:1:>',
                            Fixed: false,
                            Enabled: true,
                            Deleted: false,
                            DisplayOrder: 1
                        }
                     }";
    }
}

using arc.app.Common;

namespace arc.app.Config.Mapper.Specification;

/// <summary>
/// Mapper for adding a version number list item. Maps form data to ListItem insert.
/// ListId 135 = Version list (from breakpoint-alert-approval migration).
/// </summary>
internal class AddVersionMapper : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                        'Name': 'addversionmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            ListId: 134,
                            Value: '<:1:>',
                            Fixed: false,
                            Enabled: true,
                            Deleted: false,
                            DisplayOrder: 1
                        }
                     }";
    }
}

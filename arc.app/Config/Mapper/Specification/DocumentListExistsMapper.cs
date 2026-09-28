using arc.app.Common;

namespace arc.app.Config.Mapper.Specification;

/// <summary>
/// Maps add document form data to the documentlistitemexists query parameters.
/// ListId 135 = DocumentType list.
/// </summary>
internal class DocumentListExistsMapper : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                        'Name': 'documentlistexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            'Name': 'documentlistitemexists', 
                            'Parameters': [ 
                                {'Key': 'ListId', 'Value': '135'},
                                {'Key': 'Value', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
    }
}

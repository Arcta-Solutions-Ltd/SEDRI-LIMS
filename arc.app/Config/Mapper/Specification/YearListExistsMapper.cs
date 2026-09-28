using arc.app.Common;

namespace arc.app.Config.Mapper.Specification;

/// <summary>
/// Maps add year form data to the yearlistitemexists query parameters.
/// ListId 136 = PublicationYear list.
/// </summary>
internal class YearListExistsMapper : IDefinition
{
    /// <inheritdoc />
    public string Get()
    {
        return @"{  
                        'Name': 'yearlistexistsmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Name', Value: 'Name' }
                        ],
                        'Target' : { 
                            'Name': 'yearlistitemexists', 
                            'Parameters': [ 
                                {'Key': 'ListId', 'Value': '136'},
                                {'Key': 'Value', 'Value': '<:1:>'}
                            ] 
                        }
                     }";
    }
}

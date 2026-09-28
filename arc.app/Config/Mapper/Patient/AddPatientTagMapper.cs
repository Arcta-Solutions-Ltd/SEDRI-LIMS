using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Mapper for adding a patient tag. Maps PatientId (from record context) and ListItemId (TagId) to PatientTag insert.
/// </summary>
internal class AddPatientTagMapper : IDefinition
{
    /// <summary>
    /// Retrieves the mapper configuration.
    /// </summary>
    public string Get()
    {
        return @"{
                        'Name': 'addpatienttagmapper',
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'TagId', Value: 'TagId' }
                        ],
                        'Target': {
                            'PatientId': '<:1:>',
                            'ListItemId': '<:2:>'
                        }
                    }";
    }
}

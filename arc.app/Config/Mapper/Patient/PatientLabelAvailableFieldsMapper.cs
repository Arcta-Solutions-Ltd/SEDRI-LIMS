using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class PatientLabelAvailableFieldsMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'patientlabelavailablefieldsmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'FirstName', Value: 'FirstName' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Surname', Value: 'Surname' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'PatientRef', Value: 'PatientRef' }
                        ],
                        'Target':
                            {
                                'FirstName':'<:1:>',
                                'Surname':'<:2:>',
                                'PatientRef':'<:3:>'
                            }
                     }";
        }
    }
}


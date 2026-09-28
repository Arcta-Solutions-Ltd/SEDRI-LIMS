using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class PatientByIdMapper : IDefinition
    {
        public string Get()
        {
            return @"{
                        'Name': 'patientbyidmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'FirstName', Value: 'FirstName' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Surname', Value: 'Surname' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'Age', Value: 'Age' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'LocationId', Value: 'LocationId' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'DateOfBirth', Value: 'DateOfBirth' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'TelephoneNumber', Value: 'TelephoneNumber' },
                            { Key: '<:9:>', Type: 'Mapping', Source: 'GenderId', Value: 'GenderId' },
                            { Key: '<:10:>', Type: 'Mapping', Source: 'PatientRef', Value: 'PatientRef' },
                            { Key: '<:11:>', Type: 'Mapping', Source: 'AddressLine1', Value: 'AddressLine1' },
                            { Key: '<:12:>', Type: 'Mapping', Source: 'AddressLine2', Value: 'AddressLine2' },
                            { Key: '<:13:>', Type: 'Mapping', Source: 'ZipCode', Value: 'ZipCode' },
                            { Key: '<:14:>', Type: 'Mapping', Source: 'AgeMonths', Value: 'AgeMonths' }
                        ],
                        'Target': 
                            {
                                'FirstName':'<:1:>',
                                'Surname':'<:2:>',
                                'Age':'<:3:>',
                                'AgeMonths':'<:14:>',
                                'AddressLine1':'<:11:>',
                                'AddressLine2':'<:12:>',
                                'LocationId':'<:4:>',
                                'ZipCode':'<:13:>',
                                'DateOfBirth':'<:7:>',
                                'TelephoneNumber':'<:8:>',
                                'Gender':'<:9:>',
                                'PatientRef':'<:10:>'
                            }
                     }";
        }
    }
}

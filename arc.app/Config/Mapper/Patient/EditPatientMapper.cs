using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class EditPatientMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'editpatientmapper', 
                        'Type': 'Standard',
                        'IncludeIfNotFound': true,
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'PatientRef', Value: 'PatientRef' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'FirstName', Value: 'FirstName' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'Surname', Value: 'Surname' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'DateOfBirth', Value: 'DateOfBirth' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'Age', Value: 'Age' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'Gender', Value: 'Gender' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'TelephoneNumber', Value: 'TelephoneNumber' },
                            { Key: '<:9:>', Type: 'Mapping', Source: 'LocationId', Value: 'LocationId' },
                            { Key: '<:12:>', Type: 'Mapping', Source: 'Barcode', Value: 'Barcode' },
                            { Key: '<:13:>', Type: 'Mapping', Source: 'AddressLine1', Value: 'AddressLine1' },
                            { Key: '<:14:>', Type: 'Mapping', Source: 'AddressLine2', Value: 'AddressLine2' },
                            { Key: '<:15:>', Type: 'Mapping', Source: 'ZipCode', Value: 'ZipCode' },
                            { Key: '<:16:>', Type: 'Mapping', Source: 'AgeMonths', Value: 'AgeMonths' }
                        ],
                        'Target': 
                            {
                                'Id':'<:1:>',
                                'PatientRef':'<:2:>',
                                'FirstName':'<:3:>',
                                'Surname':'<:4:>',
                                'DateOfBirth':'<:5:>',
                                'Age':'<:6:>',
                                'AgeMonths':'<:16:>',
                                'GenderId':'<:7:>',
                                'TelephoneNumber':'<:8:>',
                                'AddressLine1':'<:13:>',
                                'AddressLine2':'<:14:>',
                                'LocationId':'<:9:>',
                                'ZipCode':'<:15:>',
                                'Barcode':'<:12:>'
                            }
                     }";
        }
    }
}

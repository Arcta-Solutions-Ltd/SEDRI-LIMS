using arc.app.Common;

namespace arc.app.Config.Mapper
{
    internal class AddPatientMapper : IDefinition
    {
        public string Get()
        {
            return @"{  
                        'Name': 'addpatientmapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Id', Value: 'Id' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'PatientRef', Value: 'PatientRef' },
                            { Key: '<:3:>', Type: 'Mapping', Source: 'FirstName', Value: 'FirstName' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'Surname', Value: 'Surname' },
                            { Key: '<:5:>', Type: 'Mapping', Source: 'DateOfBirth', Value: 'DateOfBirth' },
                            { Key: '<:6:>', Type: 'Mapping', Source: 'Age', Value: 'Age' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'Gender', Value: 'Gender' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'TelephoneNumber', Value: 'TelephoneNumber' },
                            { Key: '<:9:>', Type: 'Mapping', Source: 'ProvinceId', Value: 'ProvinceId' },
                            { Key: '<:10:>', Type: 'Mapping', Source: 'DistrictId', Value: 'DistrictId' },
                            { Key: '<:11:>', Type: 'Mapping', Source: 'SubDistrictId', Value: 'SubDistrictId' },
                            { Key: '<:12:>', Type: 'Mapping', Source: 'Barcode', Value: '<:Local.Barcode:>' }
                        ],
                        'Target': 
                            {
                                'Id':'<:1:>',
                                'PatientRef':'<:2:>',
                                'FirstName':'<:3:>',
                                'Surname':'<:4:>',
                                'DateOfBirth':'<:5:>',
                                'Age':'<:6:>',
                                'GenderId':'<:7:>',
                                'TelephoneNumber':'<:8:>',
                                'ProvinceId':'<:9:>',
                                'DistrictId':'<:10:>',
                                'SubDistrictId':'<:11:>',
                                'Barcode':'<:12:>'
                            }
                     }";
        }
    }
}

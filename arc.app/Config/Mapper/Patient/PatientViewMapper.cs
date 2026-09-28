using arc.app.Common;

namespace arc.app.Config.Mapper;

/// <summary>
/// Configuration for mapping patient view data.
/// </summary>
internal class PatientViewMapper : IDefinition
{
    /// <summary>
    /// Retrieves the configuration for mapping patient view data.
    /// </summary>
    /// <remarks>
    /// This configuration includes details such as mapping rules, target sections, 
    /// and field definitions for mapping patient-related data.
    /// </remarks>
    /// <returns>
    /// A string representation of the mapping configuration, containing metadata, 
    /// mapping rules, and target sections.
    /// </returns>
    public string Get()
    {
        return @"{
                        'Name': 'patientviewmapper',
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'FirstName', Value: 'FirstName' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Surname', Value: 'Surname' },
                            { Key: '<:4:>', Type: 'Mapping', Source: 'FullyQualifiedName', Value: 'FullyQualifiedName' },
                            { Key: '<:7:>', Type: 'Mapping', Source: 'DateOfBirth', Value: 'DateOfBirth' },
                            { Key: '<:16:>', Type: 'Mapping', Source: 'DateOfBirth', Value: 'DateOfBirth' },
                            { Key: '<:8:>', Type: 'Mapping', Source: 'TelephoneNumber', Value: 'TelephoneNumber' },
                            { Key: '<:9:>', Type: 'Mapping', Source: 'Gender', Value: 'Gender' },
                            { Key: '<:10:>', Type: 'Mapping', Source: 'PatientRef', Value: 'PatientRef' },
                            { Key: '<:11:>', Type: 'Mapping', Source: 'AddressLine1', Value: 'AddressLine1' },
                            { Key: '<:12:>', Type: 'Mapping', Source: 'AddressLine2', Value: 'AddressLine2' },
                            { Key: '<:13:>', Type: 'Mapping', Source: 'ZipCode', Value: 'ZipCode' },
                            { Key: '<:15:>', Type: 'Mapping', Source: 'tags', Value: 'tags' }
                        ],
                        'Target': {
                            Sections: [
                                {
                                    Id: 'patientdetails',
                                    Title: '',
                                    Fields: [
                                        { Id: 'firstname', Label: '@PatFir@', Highlight: true, Value: '<:1:>' },
                                        { Id: 'surname', Label: '@PatSurA@', Highlight: true, Value: '<:2:>' },
                                        { Id: 'addressline1', Label: '@PatAddD@', Value: '<:11:>' },
                                        { Id: 'addressline2', Label: '@PatAddE@', Value: '<:12:>' },
                                        { Id: 'location', Label: '@GenLoc@', Value: '<:4:>' },
                                        { Id: 'zipcode', Label: '@PatZip@', Value: '<:13:>' },
                                        { Id: 'dateofbirth', Label: '@PatDat@', Value: '<:7:>' },
                                        { Id: 'patientagedisplay', Label: '@PatAge@', Value: '<:16:>' },
                                        { Id: 'phonenumber', Label: '@PatTel@', Value: '<:8:>' },
                                        { Id: 'gender', Label: '@PatGenA@', Value: '<:9:>' },
                                        { Id: 'patientref', Label: '@PatPatB@', Value: '<:10:>' },
                                        { Id: 'tags', Label: '@GenTagA@', Value: '<:15:>' }
                                    ]
                                }
                            ]
                        }
                    }";
    }
}


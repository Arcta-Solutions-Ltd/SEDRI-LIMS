using arc.app.Common;

namespace arc.app.Config.Mapper
{
    /// <summary>
    /// Parameter mapper for the PatientSearch query, mapping workflow search form fields to query parameters.
    /// </summary>
    internal class PatientSearchParameterMapper : IDefinition
    {
        /// <summary>
        /// Returns the JSON configuration for mapping patient search form parameters to query filter values.
        /// </summary>
        /// <returns>A JSON string defining the patient search parameter mapper configuration.</returns>
        public string Get()
        {
            return @"{  
                        'Name': 'patientsearchparametermapper', 
                        'Type': 'Standard',
                        'Rules': [
                            { Key: '<:1:>', Type: 'Mapping', Source: 'Key', Where: 'PatientRefSearch', Value: 'Value' },
                            { Key: '<:2:>', Type: 'Mapping', Source: 'Key', Where: 'Values', Value: 'Value'},
                            { Key: '<:3:>', Type: 'Mapping', Source: 'Key', Where: 'SurnameSearch', Value: 'Value'},
                            { Key: '<:4:>', Type: 'Mapping', Source: 'Key', Where: 'LocationSearch', Value: 'Value'},
                            { Key: '<:5:>', Type: 'Mapping', Source: 'Key', Where: 'AgeFromYears', Value: 'Value'},
                            { Key: '<:6:>', Type: 'Mapping', Source: 'Key', Where: 'AgeFromMonths', Value: 'Value'},
                            { Key: '<:7:>', Type: 'Mapping', Source: 'Key', Where: 'AgeFromDays', Value: 'Value'},
                            { Key: '<:8:>', Type: 'Mapping', Source: 'Key', Where: 'AgeToYears', Value: 'Value'},
                            { Key: '<:9:>', Type: 'Mapping', Source: 'Key', Where: 'AgeToMonths', Value: 'Value'},
                            { Key: '<:10:>', Type: 'Mapping', Source: 'Key', Where: 'AgeToDays', Value: 'Value'},
                            { Key: '<:11:>', Type: 'Mapping', Source: 'Key', Where: 'DateOfBirthSearch', Value: 'Value'},
                            { Key: '<:12:>', Type: 'Mapping', Source: 'Key', Where: 'DateOfBirthSearch', Value: 'Value'}
                        ],
                        'Target' : { 
                            'Name': 'PatientSearch', 
                            'Parameters': [ 
                                {'Key': 'PatientRef', 'Value': '<:1:>'},
                                {'Key': 'FirstName', 'Value': '<:2:>'},
                                {'Key': 'Surname', 'Value': '<:3:>'},
                                {'Key': 'LocationId', 'Value': '<:4:>'},
                                {'Key': 'AgeFromYears', 'Value': '<:5:>'},
                                {'Key': 'AgeFromMonths', 'Value': '<:6:>'},
                                {'Key': 'AgeFromDays', 'Value': '<:7:>'},
                                {'Key': 'AgeToYears', 'Value': '<:8:>'},
                                {'Key': 'AgeToMonths', 'Value': '<:9:>'},
                                {'Key': 'AgeToDays', 'Value': '<:10:>'},
                                {'Key': 'startdate', 'Value': '<:11:>'},
                                {'Key': 'enddate', 'Value': '<:12:>'}
                            ] 
                        }
                     }";
        }
    }
}

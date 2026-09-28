using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class PatientForPatientViewQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'PatientForPatientView',
                        'TableName': 'Patient',
                        'Type': 'Single',
                        'ResultMapping': 'patientviewmapper',
                        'Fields': [
                            {'Name': 'FirstName', 'Type': 'string'},
                            {'Name': 'Surname', 'Type': 'string'},
                            {'Name': 'AddressLine1', 'Type': 'string'},
                            {'Name': 'AddressLine2', 'Type': 'string'},
                            {'Name': 'LocationId', 'Type': 'string'},
                            {'Name': 'ZipCode', 'Type': 'string'},
                            {'Name': 'DateOfBirth', 'Type': 'date' },
                            {'Name': 'TelephoneNumber', 'Type': 'string' },
                            {'Name': 'GenderId', 'Type': 'string' },
                            {'Name': 'PatientRef', 'Type': 'string' },
                            {'Name': 'Tags', 'Type': 'patienttags', 'KnownAs': 'tags' }
                        ],
                        'Joins': [
                            { 'Table': 'Location', 'Type': 'Left', 'Fields': [{'Name': 'FullyQualifiedName'}] }
                        ],
                        'ListItems': 'Gender',
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ],
                        'Translate': true,
                        'Tags': 'PS'
                    }";
        }
    }
}



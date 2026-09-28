using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class PatientByIdQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'PatientById', 'TableName': 'Patient', 'Type': 'Single', 
                        'ResultMapping': 'patientbyidmapper',
                        'Fields': [
                            {'Name': 'FirstName', 'Type': 'string'},
                            {'Name': 'Surname', 'Type': 'string'},
                            {'Name': 'Age', 'Type': 'string'},
                            {'Name': 'AgeMonths', 'Type': 'string'},
                            {'Name': 'AddressLine1', 'Type': 'string'},
                            {'Name': 'AddressLine2', 'Type': 'string'},
                            {'Name': 'LocationId', 'Type': 'string'},
                            {'Name': 'ZipCode', 'Type': 'string'},
                            {'Name': 'DateOfBirth', 'Type': 'date' },
                            {'Name': 'TelephoneNumber', 'Type': 'string' },
                            {'Name': 'GenderId', 'Type': 'string' },
                            {'Name': 'PatientRef', 'Type': 'string' }
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ],
                        'Tags': 'PS'
                    }";
        }
    }
}


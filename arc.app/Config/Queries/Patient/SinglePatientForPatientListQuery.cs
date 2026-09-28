using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SinglePatientForPatientListQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SinglePatientForPatientList',
                        'TableName': 'Patient',
                        'Type': 'Single',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'string' },
                            {'Name': 'FirstName', 'Type': 'string' },
                            {'Name': 'Surname', 'Type': 'string' },
                            {'Name': 'DateOfBirth', 'Type': 'date' },
                            {'Name': 'PatientRef', 'Type': 'string' }
                        ],
                        'Joins': [
                            { 'Table': 'Location', 'Type': 'Left', 'Fields': [{'Name': 'FullyQualifiedName'}] }
                        ],
                        'ListItems': 'Gender',
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ],
                        'Tags': 'PA'
                    }";
        }
    }
}

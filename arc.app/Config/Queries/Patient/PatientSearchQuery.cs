using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// Represents the definition of the workflow patient search query.
    /// </summary>
    internal class PatientSearchQuery : IDefinition
    {
        /// <summary>
        /// Retrieves the query definition for workflow patient search.
        /// </summary>
        /// <returns>A string containing the query definition in JSON format.</returns>
        public string Get()
        {
            return @"{ 
                        'Query': 'PatientSearch', 'TableName': 'Patient', 'Type': 'Select', 'ParameterMapping': 'patientsearchparametermapper',
                        'Fields': [
                            {'Name': 'Id', 'Type': 'string'},
                            {'Name': 'PatientRef', 'Type': 'string' },
                            {'Name': 'FirstName', 'Type': 'string'},
                            {'Name': 'Surname', 'Type': 'string'},
                            {'Name': 'DateOfBirth', 'Type': 'date' },
                        ],
                        'Joins': [
                            { 'Table': 'Location', 'Type': 'Left', 'Fields': [{'Name': 'FullyQualifiedName'}] }
                        ],
                        'ListItems': 'Gender',
                        'Where' : [
                            {'Field': 'PatientRef', 'Comparison': 'contains' },
                            {'Field': 'FirstName', 'Comparison': 'contains' },
                            {'Field': 'Surname', 'Comparison': 'contains' },
                            {'Field': 'LocationId', 'Comparison': 'in' },
                            {'Field': 'AgeFromYears', 'Comparison': 'agerange' },
                            {'Field': 'DateOfBirth', 'Comparison': 'daterange' }
                        ],
                        'OrderBy' : 'Surname',
                        'Tags': 'PS'
                    }";
        }
    }
}

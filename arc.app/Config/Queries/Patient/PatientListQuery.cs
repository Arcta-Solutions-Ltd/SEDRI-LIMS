using arc.app.Common;

namespace arc.app.Config.Queries
{
    /// <summary>
    /// This class represents the definition of the patient list query.
    /// </summary>
    internal class PatientListQuery : IDefinition
    {
        /// <summary>
        /// Retrieves the query definition for the patient list.
        /// </summary>
        /// <returns>A string containing the query definition in JSON format.</returns>
        public string Get()
        {
            return @"{
                    'Query': 'PatientList',
                    'TableName': 'Patient',
                    'Type': 'Select',
                    'Fields': [
                        {'Name': 'Id', 'Type': 'string' },
                        {'Name': 'FirstName', 'Type': 'string' },
                        {'Name': 'Surname', 'Type': 'string' },
                        {'Name': 'DateOfBirth', 'Type': 'date' },
                        {'Name': 'PatientRef', 'Type': 'string' },
                        {'Name': 'Barcode', 'Type': 'string' },
                        { 'Name': 'Tags', 'Type': 'patienttags', 'KnownAs': 'tags' }
                    ],
                    'Joins': [
                        { 'Table': 'Location', 'Type': 'Left', 'Fields': [{'Name': 'FullyQualifiedName'}] },
                        { 'Table': 'PatientTag', 'On': 'Id', 'From': 'PatientId', 'ConditionOn': 'TagId', 'CompareField': 'ListItemId' }
                    ],
                    'ListItems': 'Gender',
                    'Where': [
                        {'Field': 'GenderId', 'Comparison': 'oneof' },
                        {'Field': 'FirstName', 'Comparison': 'contains', 'orGroup': 'search' },
                        {'Field': 'Surname', 'Comparison': 'contains', 'orGroup': 'search' },
                        {'Field': 'DateOfBirth', 'Comparison': 'daterange', 'orGroup': 'search' },
                        {'Field': 'Barcode', 'Comparison': 'contains', 'orGroup': 'search' },
                        {'Field': 'PatientRef', 'Comparison': 'contains', 'orGroup': 'search' },
                        {'Field': 'FullyQualifiedName', 'Comparison': 'contains', 'orGroup': 'search' },
                        {'Field': 'Gender', 'Comparison': 'contains', 'orGroup': 'search' },
                        {'Field': 'LocationId', 'Comparison': 'in'}
                    ],
                    'Orderby': 'Surname',
                    'Tags': 'PS'
                }";
        }
    }
}

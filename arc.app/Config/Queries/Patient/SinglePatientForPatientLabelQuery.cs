using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class SinglePatientForPatientLabelQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'SinglePatientForPatientLabel',
                        'TableName': 'Patient',
                        'Type': 'Single',
                        'Fields': [
                            { 'Name': 'Id', 'Type': 'int'},
                            { 'Name': 'FirstName', 'Type': 'string' },
                            { 'Name': 'Surname', 'Type': 'string' },
                            { 'Name': 'PatientRef', 'Type': 'string' }
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ],
                        'Tags': 'PA'
                    }";
        }
    }
}

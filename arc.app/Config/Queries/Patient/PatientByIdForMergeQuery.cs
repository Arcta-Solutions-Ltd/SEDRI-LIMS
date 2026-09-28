using arc.app.Common;

namespace arc.app.Config.Queries
{
    internal class PatientByIdForMergeQuery : IDefinition
    {
        public string Get()
        {
            return @"{ 
                        'Query': 'PatientByIdForMergeQuery', 'TableName': 'Patient', 'Type': 'Single', 
                        'ResultMapping': 'patientmergemapper',
                        'Fields': [
                            {'Name': 'PatientRef', 'Type': 'string' }
                        ],
                        'Where' : [
                            {'Field': 'Id', 'Comparison': '=' } 
                        ],
                        'Tags': 'PA'
                    }";
        }
    }
}

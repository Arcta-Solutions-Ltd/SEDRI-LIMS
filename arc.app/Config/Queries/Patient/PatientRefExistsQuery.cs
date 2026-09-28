using arc.app.Common;

namespace arc.app.Config.Queries.Patient
{
    internal class PatientRefExistsQuery : IDefinition
    {
        public string Get()
        {
            return @"{
                      'Type': 'Count',
                      'Query': 'patientrefexists',
                      'Where': [
                        {
                          'Field': 'PatientRef',
                          'Comparison': 'equals'
                        },
                        {
                          'Field': 'Id', 
                          'Comparison': '!=', 
                          'FieldToMatch': 'id' }                 
                      ],
                      'Fields': [
                        {
                          'Name': 'PatientRef',
                          'Type': 'string'
                        }
                      ],
                      'TableName': 'Patient'
            }";
        }
    }
}

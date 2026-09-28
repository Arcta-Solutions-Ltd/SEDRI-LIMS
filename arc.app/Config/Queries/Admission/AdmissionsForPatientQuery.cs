using arc.app.Common;

namespace arc.app.Config.Queries.Admission
{
    /// <summary>
    /// Query definition returning the admissions held for a patient, used by the admission selection page.
    /// </summary>
    internal class AdmissionsForPatientQuery : IDefinition
    {
        /// <summary>
        /// Retrieves the query definition.
        /// </summary>
        /// <returns>A string containing the query definition in JSON format.</returns>
        public string Get()
        {
            return @"{
                'Query': 'admissionsforpatient', 'TableName': 'Admission', 'Type': 'Special'
            }";
        }
    }
}

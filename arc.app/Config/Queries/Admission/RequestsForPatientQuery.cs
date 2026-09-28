using arc.app.Common;

namespace arc.app.Config.Queries.Admission
{
    /// <summary>
    /// Query definition returning every request held for a patient, used by the request selection page when the
    /// form scopes request selection to the patient.
    /// </summary>
    internal class RequestsForPatientQuery : IDefinition
    {
        /// <summary>
        /// Retrieves the query definition.
        /// </summary>
        /// <returns>A string containing the query definition in JSON format.</returns>
        public string Get()
        {
            return @"{
                'Query': 'requestsforpatient', 'TableName': 'Request', 'Type': 'Special'
            }";
        }
    }
}

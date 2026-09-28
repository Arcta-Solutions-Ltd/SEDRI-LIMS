using arc.app.Common;

namespace arc.app.Config.Queries.Admission
{
    /// <summary>
    /// Query definition returning the requests held for a single admission, used by the request selection page
    /// when the form scopes request selection to the admission.
    /// </summary>
    internal class RequestsForAdmissionQuery : IDefinition
    {
        /// <summary>
        /// Retrieves the query definition.
        /// </summary>
        /// <returns>A string containing the query definition in JSON format.</returns>
        public string Get()
        {
            return @"{
                'Query': 'requestsforadmission', 'TableName': 'Request', 'Type': 'Special'
            }";
        }
    }
}

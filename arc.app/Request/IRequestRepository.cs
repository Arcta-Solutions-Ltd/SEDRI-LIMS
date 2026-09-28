using arc.common.Models.Requests;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Request
{
    /// <summary>
    /// Data access for the Request table, which sits between Admission and Specimen.
    /// </summary>
    public interface IRequestRepository
    {
        /// <summary>
        /// Returns the requests for a patient in reverse chronological order, for the request selection screen.
        /// </summary>
        /// <param name="queryFilters">Filter configuration carrying the patientid parameter.</param>
        /// <returns>The requests for the patient, most recent first.</returns>
        Task<List<RequestSelectionModel>> RequestsForPatientAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Returns the requests for an admission in reverse chronological order, for the request selection screen.
        /// </summary>
        /// <param name="queryFilters">Filter configuration carrying the admissionid parameter.</param>
        /// <returns>The requests for the admission, most recent first.</returns>
        Task<List<RequestSelectionModel>> RequestsForAdmissionAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Returns a single request by its primary key.
        /// </summary>
        /// <param name="queryFilters">Filter configuration carrying the id parameter.</param>
        /// <returns>The request, or an empty model when it does not exist.</returns>
        Task<RequestModel> RequestByIdAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Replaces all request file attachments with the given list of file attachment IDs.
        /// </summary>
        /// <param name="requestId">The request ID.</param>
        /// <param name="fileAttachmentIds">The file attachment IDs to link.</param>
        /// <returns>The last inserted requestfileattachments id, or 0 if none inserted.</returns>
        Task<int> SetRequestFileAttachmentsAsync(int requestId, IEnumerable<int> fileAttachmentIds);
    }
}

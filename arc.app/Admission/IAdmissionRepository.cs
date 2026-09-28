using arc.common.Models.Admissions;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Admission
{
    /// <summary>
    /// Data access for the Admission table, which sits between Patient and Request.
    /// </summary>
    public interface IAdmissionRepository
    {
        /// <summary>
        /// Returns the admissions for a patient in reverse chronological order, for the admission selection screen.
        /// </summary>
        /// <param name="queryFilters">Filter configuration carrying the patientid parameter.</param>
        /// <returns>The admissions for the patient, most recent first.</returns>
        Task<List<AdmissionSelectionModel>> AdmissionsForPatientAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Returns a single admission by its primary key.
        /// </summary>
        /// <param name="queryFilters">Filter configuration carrying the id parameter.</param>
        /// <returns>The admission, or an empty model when it does not exist.</returns>
        Task<AdmissionModel> AdmissionByIdAsync(QueryFilterConfig queryFilters);

        /// <summary>
        /// Replaces all admission file attachments with the given list of file attachment IDs.
        /// </summary>
        /// <param name="admissionId">The admission ID.</param>
        /// <param name="fileAttachmentIds">The file attachment IDs to link.</param>
        /// <returns>The last inserted admissionfileattachments id, or 0 if none inserted.</returns>
        Task<int> SetAdmissionFileAttachmentsAsync(int admissionId, IEnumerable<int> fileAttachmentIds);
    }
}
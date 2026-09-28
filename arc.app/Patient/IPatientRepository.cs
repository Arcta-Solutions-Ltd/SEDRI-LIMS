using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Patient
{
    public interface IPatientRepository
    {
        Task<SpecimenPatientModel> GetSingleCommentAsync(int id);
        Task<SpecimenPatientModel> GetSinglePatientTagAsync(int id);
        Task<int> AddPatientTagsAsync(int patientId, IEnumerable<int> listItemIds);
        Task<int> LocationPatientCountAsync(QueryFilterConfig parameters);
        Task<int> PatientRefDuplicateAsync(QueryFilterConfig parameters);
        Task<string> GetPatientRefFromIdAsync(int id);
        Task<string> GetDateOfBirthFromIdAsync(int id);
        Task<int> MergePatientAsync(string dataToSave);
        /// <summary>
        /// Replaces all patient file attachments with the given list of file attachment IDs.
        /// </summary>
        /// <param name="patientId">The patient ID.</param>
        /// <param name="fileAttachmentIds">The file attachment IDs to link.</param>
        /// <returns>The last inserted patientfileattachments id, or 0 if none inserted.</returns>
        Task<int> SetPatientFileAttachmentsAsync(int patientId, IEnumerable<int> fileAttachmentIds);
    }
}

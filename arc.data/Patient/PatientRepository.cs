using arc.app.Common;
using arc.app.Patient;
using arc.common.Models.Patient;
using arc.common.Models.Specimen;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Patient
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ISqlQuery _sqlQuery;
        private readonly ILogWriter _logWriter;
        private readonly ISqlCommand _sqlCommand;

        public PatientRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
        {
            _sqlQuery = sqlQuery;
            _logWriter = logWriter;
            _sqlCommand = sqlCommand;
        }

        public async Task<SpecimenPatientModel> GetSingleCommentAsync(int id)
        {
            var queryFilter = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "id", Value = id.ToString() } } };
            _logWriter.LogInfo("Single comment query", "PatientRepository", "GetSingleCommentAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new GetSingleCommentQuery(), "Get single patient comment query", queryFilter);
        }

        /// <summary>
        /// Retrieves specimen/patient details from a PatientTag record for workflow and alert handling.
        /// </summary>
        /// <param name="id">The PatientTag id.</param>
        /// <returns>SpecimenPatientModel with PatientId (SpecimenId will be 0).</returns>
        public async Task<SpecimenPatientModel> GetSinglePatientTagAsync(int id)
        {
            var queryFilter = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "id", Value = id.ToString() } } };
            _logWriter.LogInfo("Single patient tag query", "PatientRepository", "GetSinglePatientTagAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new GetSinglePatientTagQuery(), "Get single patient tag query", queryFilter);
        }

        /// <summary>
        /// Adds tags to a patient, skipping any that already exist.
        /// </summary>
        /// <param name="patientId">The patient ID.</param>
        /// <param name="listItemIds">The tag ListItem IDs to add.</param>
        /// <returns>The last inserted PatientTag id, or 0 if none inserted.</returns>
        public async Task<int> AddPatientTagsAsync(int patientId, IEnumerable<int> listItemIds)
        {
            var command = new AddPatientTagsModel { PatientId = patientId, ListItemIds = listItemIds };
            _logWriter.LogInfo("Add patient tags command", "PatientRepository", "AddPatientTagsAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new AddPatientTagsCommand(), "Add patient tags", command, _logWriter);
        }

        public async Task<int> LocationPatientCountAsync(QueryFilterConfig parameters)
        {
            _logWriter.LogInfo("Location patient count query", "PatientRepository", "GetSingleCommentAsync");
            return await _sqlQuery.QueryReturningIntegerAsync(new LocationPatientCountQuery(), "Location Patient Count query", parameters);
        }

        public async Task<int> PatientRefDuplicateAsync(QueryFilterConfig parameters)
        {
            _logWriter.LogInfo("PatientRef duplicate query", "PatientRepository", "GetSingleCommentAsync");
            return await _sqlQuery.QueryReturningIntegerAsync(new PatientRefDuplicateQuery(), "PatientRef Duplicate query", parameters);
        }

        public async Task<string> GetPatientRefFromIdAsync(int id)
        {
            var queryFilter = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "id", Value = id.ToString() } } };
            _logWriter.LogInfo("Single comment query", "PatientRepository", "GetSingleCommentAsync");
            return await _sqlQuery.QueryReturningStringAsync(new GetPatientRefFromIdQuery(), "Get single patient comment query", queryFilter);
        }

        /// <summary>
        /// Retrieves a patient's date of birth by ID for specimen age auto-calculation.
        /// </summary>
        /// <param name="id">Patient ID.</param>
        /// <returns>Date of birth as ISO string (yyyy-MM-dd), or null if not set.</returns>
        public async Task<string> GetDateOfBirthFromIdAsync(int id)
        {
            var queryFilter = new QueryFilterConfig { Parameters = new List<QueryValuesConfig> { new QueryValuesConfig { Key = "id", Value = id.ToString() } } };
            _logWriter.LogInfo("Get date of birth by patient id", "PatientRepository", "GetDateOfBirthFromIdAsync");
            return await _sqlQuery.QueryReturningStringAsync(new GetDateOfBirthFromIdQuery(), "Get date of birth by patient id", queryFilter);
        }

        public async Task<int> MergePatientAsync(string dataToSave)
        {
            var patientInfo = JsonConvert.DeserializeObject<MergePatientModel>(dataToSave);
            _logWriter.LogInfo("Merge patient command", "PatientRepository", "MergePatientAsync");
            return await _sqlCommand.CommandWithTypeQueryAsync(new MergePatientCommand(), "Merge  Patient", patientInfo);
        }

        /// <summary>
        /// Replaces all patient file attachments with the given list of file attachment IDs.
        /// Deletes existing links for the patient, then inserts the new set.
        /// </summary>
        /// <param name="patientId">The patient ID.</param>
        /// <param name="fileAttachmentIds">The file attachment IDs to link.</param>
        /// <returns>The last inserted patientfileattachments id, or 0 if none inserted.</returns>
        public async Task<int> SetPatientFileAttachmentsAsync(int patientId, IEnumerable<int> fileAttachmentIds)
        {
            var model = new SetPatientFileAttachmentsModel
            {
                PatientId = patientId,
                FileAttachmentIds = fileAttachmentIds
            };
            return await _sqlCommand.CommandWithTypeQueryAsync(
                new SetPatientFileAttachmentsCommand(),
                "Set Patient File Attachments",
                model,
                _logWriter);
        }
    }
}

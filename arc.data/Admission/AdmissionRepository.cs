using arc.app.Admission;
using arc.app.Common;
using arc.common.Models.Admissions;
using arc.data.Common;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Admission
{
    /// <summary>
    /// Data access for the Admission table.
    /// </summary>
    /// <param name="sqlQuery">Wrapper used to run queries against the database.</param>
    /// <param name="logWriter">Log writer used to record repository activity.</param>
    /// <param name="sqlCommand">Wrapper used to run commands against the database.</param>
    public class AdmissionRepository(ISqlQuery sqlQuery, ILogWriter logWriter, ISqlCommand sqlCommand)
        : GeneralRepository(sqlQuery, logWriter, sqlCommand), IAdmissionRepository
    {
        /// <inheritdoc />
        public async Task<List<AdmissionSelectionModel>> AdmissionsForPatientAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run admissions for patient query", "AdmissionRepository", "AdmissionsForPatientAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new AdmissionsForPatientQuery(), "Get admissions for patient", queryFilters);
        }

        /// <inheritdoc />
        public async Task<AdmissionModel> AdmissionByIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run admission by id query", "AdmissionRepository", "AdmissionByIdAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new AdmissionByIdQuery(), "Get admission by id", queryFilters);
        }

        /// <inheritdoc />
        public async Task<int> SetAdmissionFileAttachmentsAsync(int admissionId, IEnumerable<int> fileAttachmentIds)
        {
            var model = new SetAdmissionFileAttachmentsModel
            {
                AdmissionId = admissionId,
                FileAttachmentIds = fileAttachmentIds
            };
            return await _sqlCommand.CommandWithTypeQueryAsync(
                new SetAdmissionFileAttachmentsCommand(),
                "Set Admission File Attachments",
                model,
                _logWriter);
        }
    }
}

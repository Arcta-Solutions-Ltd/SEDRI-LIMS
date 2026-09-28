using arc.app.Common;
using arc.app.Request;
using arc.common.Models.Requests;
using arc.data.Common;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Request
{
    /// <summary>
    /// Data access for the Request table.
    /// </summary>
    /// <param name="sqlQuery">Wrapper used to run queries against the database.</param>
    /// <param name="logWriter">Log writer used to record repository activity.</param>
    /// <param name="sqlCommand">Wrapper used to run commands against the database.</param>
    public class RequestRepository(ISqlQuery sqlQuery, ILogWriter logWriter, ISqlCommand sqlCommand)
        : GeneralRepository(sqlQuery, logWriter, sqlCommand), IRequestRepository
    {
        /// <inheritdoc />
        public async Task<List<RequestSelectionModel>> RequestsForPatientAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run requests for patient query", "RequestRepository", "RequestsForPatientAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new RequestsForPatientQuery(), "Get requests for patient", queryFilters);
        }

        /// <inheritdoc />
        public async Task<List<RequestSelectionModel>> RequestsForAdmissionAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run requests for admission query", "RequestRepository", "RequestsForAdmissionAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new RequestsForAdmissionQuery(), "Get requests for admission", queryFilters);
        }

        /// <inheritdoc />
        public async Task<RequestModel> RequestByIdAsync(QueryFilterConfig queryFilters)
        {
            _logWriter.LogInfo("Run request by id query", "RequestRepository", "RequestByIdAsync");
            return await _sqlQuery.QueryReturningTypeAsync(new RequestByIdQuery(), "Get request by id", queryFilters);
        }

        /// <inheritdoc />
        public async Task<int> SetRequestFileAttachmentsAsync(int requestId, IEnumerable<int> fileAttachmentIds)
        {
            var model = new SetRequestFileAttachmentsModel
            {
                RequestId = requestId,
                FileAttachmentIds = fileAttachmentIds
            };
            return await _sqlCommand.CommandWithTypeQueryAsync(
                new SetRequestFileAttachmentsCommand(),
                "Set Request File Attachments",
                model,
                _logWriter);
        }
    }
}

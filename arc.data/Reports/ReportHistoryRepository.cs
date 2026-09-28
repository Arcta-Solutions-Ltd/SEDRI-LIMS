using arc.app.Common;
using arc.app.Reports;
using arc.common.Models.Reports;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Reports;

/// <summary>
/// Provides data access methods for report history operations.
/// </summary>
public class ReportHistoryRepository : IReportHistoryRepository
{
    private readonly ISqlCommand _sqlCommand;
    private readonly ISqlQuery _sqlQuery;
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReportHistoryRepository"/> class.
    /// </summary>
    /// <param name="sqlCommand">The SQL command executor.</param>
    /// <param name="sqlQuery">The SQL query executor.</param>
    /// <param name="logWriter">The logger for writing informational messages.</param>
    public ReportHistoryRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
    {
        _sqlCommand = sqlCommand;
        _sqlQuery = sqlQuery;
        _logWriter = logWriter;
    }

    /// <summary>
    /// Inserts a new report history record asynchronously.
    /// </summary>
    /// <param name="dataToSave">The <see cref="ReportHistory"/> object to save.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the number of affected rows.</returns>
    public async Task<int> AddReportHistoryAsync(ReportHistory dataToSave)
    {
        _logWriter.LogInfo("Run add report history command", "ReportHistoryRepository", "AddReportHistoryAsync");
        return await _sqlCommand.CommandWithTypeQueryAsync(new AddReportHistoryCommand(), "Insert Report", dataToSave);
    }

    /// <summary>
    /// Retrieves the contents of a report from history asynchronously.
    /// </summary>
    /// <param name="queryFilters">The filter configuration containing query parameters.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains a <see cref="ReportHistory"/> populated with report contents.
    /// </returns>
    public async Task<ReportHistory> GetReportContentsAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run get report contents query", "ReportHistoryRepository", "GetReportContentsAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new ReportContentsFromHistoryQuery(), "Get Report Contents from History", queryFilters);
    }

    /// <summary>
    /// Retrieves a list of patient report models asynchronously.
    /// </summary>
    /// <param name="queryFilters">The filter configuration containing query parameters.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. 
    /// The task result contains a list of <see cref="PatientReportListModel"/> instances.
    /// </returns>
    public async Task<List<PatientReportListModel>> GetPatientReportListAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run patient report list query", "ReportHistoryRepository", "GetPatientReportListAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new PatientReportListQuery(_logWriter), "Get patient report list", queryFilters);
    }

    /// <summary>
    /// Retrieves report history rows for specimens linked to an admission.
    /// </summary>
    /// <param name="queryFilters">The filter configuration containing the admission id parameter.</param>
    /// <returns>Report history list models for the admission scope.</returns>
    public async Task<List<PatientReportListModel>> GetAdmissionReportListAsync(QueryFilterConfig queryFilters)
    {
        var admissionId = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value;
        _logWriter.LogInfo(
            $"Run admission report list query for admission id {admissionId ?? "(missing)"}",
            nameof(ReportHistoryRepository),
            nameof(GetAdmissionReportListAsync));
        var result = await _sqlQuery.QueryReturningTypeAsync(new AdmissionReportListQuery(_logWriter), "Get admission report list", queryFilters);
        _logWriter.LogInfo(
            $"Admission report list query returned {result.Count} row(s) for admission id {admissionId ?? "(missing)"}",
            nameof(ReportHistoryRepository),
            nameof(GetAdmissionReportListAsync));
        return result;
    }

    /// <summary>
    /// Retrieves report history rows for specimens linked to a request.
    /// </summary>
    /// <param name="queryFilters">The filter configuration containing the request id parameter.</param>
    /// <returns>Report history list models for the request scope.</returns>
    public async Task<List<PatientReportListModel>> GetRequestReportListAsync(QueryFilterConfig queryFilters)
    {
        var requestId = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("id", StringComparison.OrdinalIgnoreCase))?.Value;
        _logWriter.LogInfo(
            $"Run request report list query for request id {requestId ?? "(missing)"}",
            nameof(ReportHistoryRepository),
            nameof(GetRequestReportListAsync));
        var result = await _sqlQuery.QueryReturningTypeAsync(new RequestReportListQuery(_logWriter), "Get request report list", queryFilters);
        _logWriter.LogInfo(
            $"Request report list query returned {result.Count} row(s) for request id {requestId ?? "(missing)"}",
            nameof(ReportHistoryRepository),
            nameof(GetRequestReportListAsync));
        return result;
    }
}

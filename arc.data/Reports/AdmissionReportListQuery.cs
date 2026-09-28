using arc.app.Common;
using arc.common.Models.Reports;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Reports;

/// <summary>
/// Returns report history rows for specimens belonging to a single admission.
/// </summary>
internal class AdmissionReportListQuery : IQueryReturningType<List<PatientReportListModel>>
{
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initialises a new instance of the <see cref="AdmissionReportListQuery"/> class.
    /// </summary>
    /// <param name="logWriter">Logger for support diagnostics.</param>
    public AdmissionReportListQuery(ILogWriter logWriter)
    {
        _logWriter = logWriter;
    }

    /// <summary>
    /// Runs the query against the supplied connection.
    /// </summary>
    /// <param name="connect">An open connection to the database.</param>
    /// <param name="queryFilters">Filter configuration carrying the admission id parameter (<c>id</c>).</param>
    /// <returns>Distinct report history rows for specimens linked to the admission.</returns>
    public async Task<List<PatientReportListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var idParameter = queryFilters.Parameters.FirstOrDefault(p => p.Key.ToLower() == "id");
        if (idParameter == null || !int.TryParse(idParameter.Value, out var admissionId))
        {
            _logWriter.LogInfo(
                "Admission report list query skipped: missing or invalid id parameter",
                nameof(AdmissionReportListQuery),
                nameof(ExecuteAsync));
            return [];
        }

        var sql = $"""
            {ReportHistoryListSql.SelectList}
            WHERE a.AdmissionId = @Id
            {ReportHistoryListSql.OrderByLastModifiedDate}
            """;

        var result = (await connect.QueryAsync<PatientReportListModel>(sql, new { Id = admissionId })).ToList();
        _logWriter.LogInfo(
            $"Admission report list query returned {result.Count} row(s) for admission id {admissionId}",
            nameof(AdmissionReportListQuery),
            nameof(ExecuteAsync));
        return result;
    }
}

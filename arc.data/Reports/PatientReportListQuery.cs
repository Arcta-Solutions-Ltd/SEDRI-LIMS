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
/// Returns report history rows for specimens belonging to a single patient.
/// </summary>
internal class PatientReportListQuery : IQueryReturningType<List<PatientReportListModel>>
{
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initialises a new instance of the <see cref="PatientReportListQuery"/> class.
    /// </summary>
    /// <param name="logWriter">Logger for support diagnostics.</param>
    public PatientReportListQuery(ILogWriter logWriter)
    {
        _logWriter = logWriter;
    }

    /// <summary>
    /// Runs the query against the supplied connection.
    /// </summary>
    /// <param name="connect">An open connection to the database.</param>
    /// <param name="queryFilters">Filter configuration carrying the patient id parameter (<c>id</c>).</param>
    /// <returns>Distinct report history rows for specimens linked to the patient.</returns>
    public async Task<List<PatientReportListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var idParameter = queryFilters.Parameters.FirstOrDefault(p => p.Key.ToLower() == "id");
        if (idParameter == null || !int.TryParse(idParameter.Value, out var patientId))
        {
            _logWriter.LogInfo(
                "Patient report list query skipped: missing or invalid id parameter",
                nameof(PatientReportListQuery),
                nameof(ExecuteAsync));
            return [];
        }

        var sql = $"""
            {ReportHistoryListSql.SelectList}
            WHERE a.PatientId = @Id
            {ReportHistoryListSql.OrderByLastModifiedDate}
            """;

        var result = (await connect.QueryAsync<PatientReportListModel>(sql, new { Id = patientId })).ToList();
        _logWriter.LogInfo(
            $"Patient report list query returned {result.Count} row(s) for patient id {patientId}",
            nameof(PatientReportListQuery),
            nameof(ExecuteAsync));
        return result;
    }
}

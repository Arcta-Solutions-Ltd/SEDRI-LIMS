using arc.app.Common;
using arc.app.Reports;
using arc.app.Reports.InclusionSelectors;
using arc.common.Models.Reports;
using arc.domain.Configuration.QueryFiltersConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Reports;

/// <summary>
/// Repository implementation for handling report-related database operations.
/// Utilizes SQL command and query abstractions for data access.
/// </summary>
public class ReportRepository : IReportRepository
{
    private readonly ISqlCommand _sqlCommand;
    private readonly ISqlQuery _sqlQuery;
    private readonly ILogWriter _logWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ReportRepository"/> class with required dependencies.
    /// </summary>
    /// <param name="sqlCommand">SQL command handler for executing write operations.</param>
    /// <param name="sqlQuery">SQL query handler for retrieving data.</param>
    /// <param name="logWriter">Logger for recording operational messages.</param>
    public ReportRepository(ISqlCommand sqlCommand, ISqlQuery sqlQuery, ILogWriter logWriter)
    {
        _sqlQuery = sqlQuery;
        _sqlCommand = sqlCommand;
        _logWriter = logWriter;
    }

    /// <summary>
    /// Updates the report with selected culture details.
    /// </summary>
    /// <param name="dataToSave">The culture detail model to be saved to the report.</param>
    public async Task UpdateCultureDisplayOnReportAsync(CultureDetailsSelectorModel dataToSave)
    {
        _logWriter.LogInfo("Update culture print selector display flags", "ReportRepository", nameof(UpdateCultureDisplayOnReportAsync));
        await _sqlCommand.CommandWithTypeQueryAsync(new CultureDetailsSelectorCommand(), "Update culture details selector", dataToSave);
    }

    /// <summary>
    /// Retrieves antibiogram data based on the given query filters.
    /// </summary>
    /// <param name="queryFilters">Filter configuration for the query.</param>
    /// <returns>A list of antibiogram result models.</returns>
    public async Task<List<AntibiogramModel>> GetAntibiogramDataAsync(QueryFilterConfig queryFilters)
    {
        var locationId = queryFilters.Parameters
            .FirstOrDefault(p => p.Key.Equals("locationid", StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;
        var organisationFilterId = queryFilters.Parameters
            .FirstOrDefault(p => p.Key.Equals("organisationfilterid", StringComparison.OrdinalIgnoreCase))?.Value ?? string.Empty;
        _logWriter.LogInfo(
            $"Run antibiogram query (locationid={locationId}, organisationfilterid={organisationFilterId})",
            "ReportRepository",
            nameof(GetAntibiogramDataAsync));
        return await _sqlQuery.QueryReturningTypeAsync(new AntibiogramQuery(), "Get antibiogram data", queryFilters);
    }

    /// <summary>
    /// Retrieves a list of approved reports using the provided filters.
    /// </summary>
    /// <param name="queryFilters">Filter configuration for approved reports.</param>
    /// <returns>A list of approved reports.</returns>
    public async Task<List<ApprovedReportsListModel>> GetApprovedReportsListAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run approved reports list query", "ReportRepository", "GetApprovedReportsListAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new ApprovedReportsListQuery(), "Get approved reports list", queryFilters);
    }

    /// <summary>
    /// Retrieves a list of approved report history entries filtered by the specified criteria.
    /// </summary>
    /// <param name="queryFilters">
    /// Configuration object containing filter criteria, paging, and sorting options
    /// for the approved reports query.
    /// </param>
    /// <returns>
    /// An <see cref="ApprovedReportsListModel"/> containing the filtered approved reports.
    /// </returns>
    public async Task<ApprovedReportsListModel> GetApprovedReportsListByIdAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run approved reports list by id query", "ReportRepository", "GetApprovedReportsListByIdAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new ApprovedReportListByIdQuery(), "Get approved reports list by id", queryFilters);
    }

    /// <summary>
    /// Executes the approve report form query and retrieves the corresponding approval form model.
    /// </summary>
    /// <param name="queryFilters">
    /// Configuration containing the filter criteria for the query.
    /// </param>
    /// <returns>
    /// A task that returns the <see cref="ApproveReportFormModel"/> result.
    /// </returns>
    public async Task<ApproveReportFormModel> GetApproveReportFormQueryAsync(QueryFilterConfig queryFilters)
    {
        _logWriter.LogInfo("Run approve report form query", "ReportRepository", "GetApproveReportFormQueryAsync");
        return await _sqlQuery.QueryReturningTypeAsync(new ApproveReportFormQuery(), "Get approve report form query", queryFilters);
    }

    /// <summary>
    /// Updates the approval status of a report history entry, recording who made the decision.
    /// Used for both approving and rejecting reports.
    /// </summary>
    /// <param name="id">
    /// The identifier of the report history entry to update.
    /// </param>
    /// <param name="username">
    /// The username of the person approving or rejecting the report.
    /// Stored in <c>reporthistory.approvedby</c> and surfaced as the "Decision By" column in the list view.
    /// </param>
    /// <param name="approval">
    /// The decision string: <c>"Yes"</c> to approve (sets state 123) or <c>"No"</c> to reject (sets state 124).
    /// </param>
    /// <returns>
    /// A <see cref="Task"/> that completes when the approval change operation has finished.
    /// </returns>
    public async Task ApprovalChangeAsync(string id, string username, string approval)
    {
        var reportApprovalListId = string.Equals(approval, "Yes", StringComparison.Ordinal) ? 123 : 124;
        var usernameMissing = string.IsNullOrWhiteSpace(username);
        _logWriter.LogInfo(
            $"Approval change: reportHistoryId={id}; approval={approval}; reportApprovalListId={reportApprovalListId}; usernameMissing={usernameMissing}",
            "ReportRepository",
            "ApprovalChangeAsync");
        await _sqlCommand.CarryOutCommandAsync(new ApprovalChangeCommand(), "Approval change", id, approval, username);
    }
}



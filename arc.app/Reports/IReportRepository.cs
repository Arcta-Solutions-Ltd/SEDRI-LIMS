using arc.app.Reports.InclusionSelectors;
using arc.common.Models.Reports;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.app.Reports;

/// <summary>
/// Defines methods for interacting with report-related data in the system.
/// </summary>
public interface IReportRepository
{
    /// <summary>
    /// Updates the culture display information on a report based on provided data.
    /// </summary>
    /// <param name="dataToSave">Data model containing the culture details to be saved.</param>
    Task UpdateCultureDisplayOnReportAsync(CultureDetailsSelectorModel dataToSave);

    /// <summary>
    /// Retrieves antibiogram data based on query filter criteria.
    /// </summary>
    /// <param name="queryFilters">Filter configuration used to retrieve relevant antibiogram results.</param>
    /// <returns>A list of antibiogram models.</returns>
    Task<List<AntibiogramModel>> GetAntibiogramDataAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Retrieves a list of approved reports that match the specified query filters.
    /// </summary>
    /// <param name="queryFilters">Filter configuration for retrieving approved reports.</param>
    /// <returns>A list of approved report list models.</returns>
    Task<List<ApprovedReportsListModel>> GetApprovedReportsListAsync(QueryFilterConfig queryFilters);

    /// <summary>
    /// Executes the approve report form query and retrieves the corresponding approval form model.
    /// </summary>
    /// <param name="queryFilters">
    /// Configuration containing the filter criteria for the query.
    /// </param>
    /// <returns>
    /// A task that returns the <see cref="ApproveReportFormModel"/> result.
    /// </returns>
    Task<ApproveReportFormModel> GetApproveReportFormQueryAsync(QueryFilterConfig queryFilters);

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
    Task ApprovalChangeAsync(string id, string username, string approval);

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
    Task<ApprovedReportsListModel> GetApprovedReportsListByIdAsync(QueryFilterConfig queryFilters);
}

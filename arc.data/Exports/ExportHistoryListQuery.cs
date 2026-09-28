using arc.common.ExtensionMethods;
using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports;

/// <summary>
/// Retrieves export history list with export profile name and schedule name.
/// Supports filtering by one or more export profile IDs.
/// </summary>
internal class ExportHistoryListQuery : IQueryReturningType<List<ExportHistoryModel>>
{
    /// <summary>
    /// Executes the export history list query with optional export profile filter.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Optional filters: "exportprofileid" (comma-separated profile IDs).</param>
    /// <returns>List of export history rows ordered by run date descending.</returns>
    public async Task<List<ExportHistoryModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var hasExportProfileFilter = queryFilters.TryGetStringValue("exportprofileid", out var exportProfileIdValue, "");
        var exportProfileIds = exportProfileIdValue.ToIntList().ToArray();

        var whereClause = "";
        object parameters = null;

        if (hasExportProfileFilter && exportProfileIds.Length > 0)
        {
            whereClause = " WHERE erh.exportprofileid = ANY(@exportProfileIds)";
            parameters = new { exportProfileIds };
        }

        var sql = $@"
            SELECT erh.id, erh.exportprofileid, ep.name AS exportprofilename, erh.runat, erh.filter, erh.fileattachmentid,
                erh.exportscheduleid, es.name AS schedulename
            FROM exportrunhistory erh
            LEFT JOIN exportprofile ep ON ep.id = erh.exportprofileid
            LEFT JOIN exportschedule es ON es.id = erh.exportscheduleid
            {whereClause}
            ORDER BY erh.runat DESC";

        var result = parameters != null
            ? await connect.QueryAsync<ExportHistoryModel>(sql, parameters)
            : await connect.QueryAsync<ExportHistoryModel>(sql);
        return result.ToList();
    }
}

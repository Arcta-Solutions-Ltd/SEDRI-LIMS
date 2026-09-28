using arc.common.ExtensionMethods;
using arc.common.Models.Reports;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Reports;

/// <summary>
/// Defines a query for retrieving approved reports based on provided filters.
/// </summary>
internal class ApprovedReportsListQuery : IQueryReturningType<List<ApprovedReportsListModel>>
{
    /// <summary>
    /// Executes the query asynchronously to fetch approved reports.
    /// </summary>
    /// <param name="connect">The database connection to use for the query.</param>
    /// <param name="queryFilters">The set of filters to apply when querying.</param>
    /// <returns>
    /// A list of <see cref="ApprovedReportsListModel"/> instances matching the filter criteria.
    /// </returns>
    public async Task<List<ApprovedReportsListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        _ = queryFilters.TryParseDateValue("startdate", out var startDate, new DateTime());
        _ = queryFilters.TryParseDateValue("enddate", out var endDate, new DateTime());
        var hasAccessionNumberMatch = queryFilters.TryGetStringValue("accessionnumber", out var accessionNumber, "");
        var hasApprovalFilter = queryFilters.TryGetStringValue("approvedid", out var approvedid);
        var hasApprovedFilter = queryFilters.TryGetStringValue("approved", out var approved);
        _ = queryFilters.TryGetStringValue("surname", out var surname, "");

        var whereClause = await SpecimenFilter.GetWhereClauseAsync(queryFilters, connect, "rh.requesteddate");
        if (hasAccessionNumberMatch) { whereClause += " and (s.accessionnumber ilike '%' || @AccessionNumber || '%' or p.surname ilike '%' || @Surname || '%')";  }

        if (hasApprovedFilter && approved == "Yes")
        {
            whereClause += " and rh.reportapprovalid in (123)";
        }
        else
        {
            // Use sanitized int literals — Dapper binds @ApprovedId as text, which breaks rh.reportapprovalid (int).
            if (hasApprovalFilter)
            {
                var approvedIdsSql = SqlSanitizer.SanitizeIdList(approvedid);
                if (approvedIdsSql != "NULL")
                {
                    whereClause += " and rh.reportapprovalid in (" + approvedIdsSql + ")";
                }
            }
        }

        var join = await SpecimenFilter.GetJoinClauseAsync(queryFilters, connect);

        var order = "rh.approvaldate desc";
        if (!string.IsNullOrWhiteSpace(queryFilters.OrderBy))
        {
            var desc = queryFilters.OrderDescending ? " desc" : "";
            order = queryFilters.OrderBy + desc;
        }

        var sql = $"""
            select rh.id, s.accessionnumber, p.surname, li.value as specimentype,og.organisationname,
            rh.approvaldate AT TIME ZONE 'UTC' As approvaldate,
            rh.approvedby, rh.requesteddate AT TIME ZONE 'UTC' As requesteddate,
            li2.value as approved, rh.reportapprovalid as stateid
            from reporthistory rh
            inner join specimen s on s.id = rh.specimenid
            inner join listitem li on li.id = s.specimentypeid
            inner join listitem li2 on li2.id = rh.reportapprovalid
            inner join organisation og on og.id = s.organisationid
            {join}
            {whereClause}
            order by {order}
            limit 1000
            """;

        var result = await connect.QueryAsync<ApprovedReportsListModel>(sql, new { startDate, endDate, accessionNumber, surname });
        return [.. result];
    }
}

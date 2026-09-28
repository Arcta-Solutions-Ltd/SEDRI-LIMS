using arc.common.Models.Graph;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Graph;

/// <summary>
/// Graph query aggregating specimen counts by workflow state (list 60), with date interval grouping and specimen filters.
/// </summary>
internal class SpecimenStateGraph : IQueryReturningType<List<GraphModel>>
{
    /// <inheritdoc />
    public async Task<List<GraphModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var dateSelector = new DateSelection();
        // Qualify with s. — patient (p) also has stateid, so bare "stateid" is ambiguous in GROUP BY.
        dateSelector.IncorporateDateInterval(queryFilters, "s.stateid", "li.value");

        _ = queryFilters.TryParseDateValue("startdate", out var startDate, new DateTime());
        _ = queryFilters.TryParseDateValue("enddate", out var endDate, new DateTime());

        var dynamicWhere = await SpecimenFilter.GetWhereClauseAsync(queryFilters, connect);
        var join = await SpecimenFilter.GetJoinClauseAsync(queryFilters, connect);

        const string stateExclude = "s.stateid not in (534, 537, 528)";
        var whereClause = string.IsNullOrWhiteSpace(dynamicWhere)
            ? "where " + stateExclude
            : dynamicWhere + " and " + stateExclude;

        var sql = $"""
            with tablevalues as
                (
                    select s.stateid, count(*) as number, {dateSelector.Select}
                    from specimen s
                    {join}
                    {whereClause}
                    group by {dateSelector.GroupBy}
                )
            select {dateSelector.TableSelection}
            from tablevalues tv
            inner join listitem li on tv.stateid = li.id
            where li.listid = 60
            order by {dateSelector.OrderBy}
            """;

        var result = await connect.QueryAsync<GraphModel>(sql, new { startDate, endDate });
        return result.ToList();
    }
}

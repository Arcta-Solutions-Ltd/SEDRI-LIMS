using arc.common.Models.Graph;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Graph;

internal class TestGraph : IQueryReturningType<List<GraphModel>>
{
    public async Task<List<GraphModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var dateSelector = new DateSelection();
        dateSelector.IncorporateDateInterval(queryFilters, "tt.testname", "tv.testname as value");

        _ = queryFilters.TryParseDateValue("startdate", out var startDate, new DateTime());
        _ = queryFilters.TryParseDateValue("enddate", out var endDate, new DateTime());

        var whereClause = await SpecimenFilter.GetWhereClauseAsync(queryFilters, connect);
        var join = await SpecimenFilter.GetJoinClauseAsync(queryFilters, connect, false, false, false, true);

        var sql = $"""
            with tablevalues as
                (
                    select tt.testname, count(*) as number, {dateSelector.Select}
                    from specimen s
                    {join}
                    {whereClause}
                    group by {dateSelector.GroupBy}
                )
            select {dateSelector.TableSelection} 
            from tablevalues tv
            order by {dateSelector.OrderBy}
            """;

        var result = await connect.QueryAsync<GraphModel>(sql, new { startDate, endDate });

        return [.. result];
    }
}


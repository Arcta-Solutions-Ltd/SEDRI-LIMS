using arc.common.Models.Specimen;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Specimen;

internal class SpecimenTypeCountQuery : IQueryReturningType<List<SpecimenCountModel>>
{
    public async Task<List<SpecimenCountModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var dynamicWhere = await SpecimenFilter.GetWhereClauseAsync(queryFilters, connect);
        var join = await SpecimenFilter.GetJoinClauseAsync(queryFilters, connect);

        const string stateExclude = "s.stateid not in (534, 537, 528)";
        var whereClause = string.IsNullOrWhiteSpace(dynamicWhere)
            ? "where " + stateExclude
            : dynamicWhere + " and " + stateExclude;

        _ = queryFilters.TryParseDateValue("startdate", out var startDate, default);
        _ = queryFilters.TryParseDateValue("enddate", out var endDate, default);

        var sql = $"""
            with totals as (
                select s.specimentypeid, count(*) as number
                from specimen s
                {join}
                {whereClause}
                group by s.specimentypeid
            )
            select li.id, li.value, t.number
            from listitem li
            inner join totals t on t.specimentypeid = li.id
            where li.listid = 4
            order by li.value
            """;

        var result = await connect.QueryAsync<SpecimenCountModel>(sql, new { startDate, endDate });
        return [.. result];
    }
}

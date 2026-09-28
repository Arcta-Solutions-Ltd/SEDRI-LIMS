using arc.common.Models.Quality;
using arc.data.Utils;
using arc.data.Extensions;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace arc.data.Quality.Queries;

internal class IqcTestsListQuery : IQueryReturningType<List<IqcTestListModel>>
{
    public async Task<List<IqcTestListModel>> ExecuteAsync(NpgsqlConnection connection, QueryFilterConfig queryFilters)
    {
        var whereClause = new StringBuilder($"");

        if (queryFilters.TryParseIntegerValue("stateid", out var stateId))
        {
            whereClause.AppendLine(" and it.stateid = @stateId");
        }

        if (queryFilters.TryGetStringValue("accessionnumber", out var accessionNumber))
        {
            queryFilters.TryGetStringValue("status", out var status);
            whereClause.AppendLine($"""
                            and (it.accessionnumber ilike '{accessionNumber.Trim().ToSqlWildcard()}'
                            or li.value ilike '{status.Trim().ToSqlStartsWith()}')
                            """);
        }

        if (queryFilters.TryParseDateValue("startdate", out var startDate))
        {
            whereClause.AppendLine(" and (it.createddate >= @startDate or it.completeddate >= @startDate)");
        }

        if (queryFilters.TryParseDateValue("endDate", out var endDate))
        {
            whereClause.AppendLine(" and (it.createddate <= @endDate or it.completeddate <= @endDate)");
        }

        var sql = $"""
                select 
                it.id,
                it.accessionnumber,
                it.stateid,
                li.value as state,
                to_char(timezone('UTC',it.createddate::timestamptz), 'YYYY-MM-DD""T""HH:MI:SS') as createddate,
                to_char(timezone('UTC',it.completeddate::timestamptz), 'YYYY-MM-DD""T""HH:MI:SS') as completeddate
                from iqctests it
                inner join listitem li on li.id = it.stateid
                where 1=1
                {whereClause} 
                order by {ListQueryOrderByUtil.GetOrderByClause(queryFilters, "it.id")}
                """;

        var result = await connection.QueryAsync<IqcTestListModel>(sql,
            new { startDate, endDate, stateId, accessionNumber });
        return result.ToList();
    }
}

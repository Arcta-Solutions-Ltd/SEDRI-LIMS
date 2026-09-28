using arc.common.Models.Graph;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Graph;

/// <summary>
/// Provides a query implementation to fetch graph data aggregated by specimen type,
/// applying date interval grouping and specimen filters.
/// </summary>
internal class SpecimenTypeGraph : IQueryReturningType<List<GraphModel>>
{
    /// <summary>
    /// Executes the specimen type graph query against the database:
    /// applies date interval grouping, retrieves specimen filters, groups by specimen type and date,
    /// and joins with the listitem table to obtain the display labels.
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> used to execute the SQL query.
    /// </param>
    /// <param name="queryFilters">
    /// Filter configuration including date interval (startdate, enddate), specimen type, location, and other parameters.
    /// </param>
    /// <returns>
    /// A list of <see cref="GraphModel"/> instances, each containing the specimen type label,
    /// its count for the interval, and the corresponding date grouping.
    /// </returns>
    public async Task<List<GraphModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var dateSelector = new DateSelection();
        dateSelector.IncorporateDateInterval(queryFilters, "specimentypeid", "li.value");

        _ = queryFilters.TryParseDateValue("startdate", out var startDate, new DateTime());
        _ = queryFilters.TryParseDateValue("enddate", out var endDate, new DateTime());

        var whereClause = await SpecimenFilter.GetWhereClauseAsync(queryFilters, connect);
        var join = await SpecimenFilter.GetJoinClauseAsync(queryFilters, connect);

        var sql = $"""
            with tablevalues as
                (
                    select s.specimentypeId, count(*) as number, {dateSelector.Select}
                    from specimen s {join} {whereClause}
                    group by {dateSelector.GroupBy}
                )
            select {dateSelector.TableSelection} 
            from tablevalues tv
            inner join listitem li on tv.specimentypeid = li.id
            order by {dateSelector.OrderBy};
            """;

        var result = await connect.QueryAsync<GraphModel>(sql, new { startDate, endDate });
        return [.. result];
    }
}



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
/// Provides a query implementation to fetch gender-distributed graph data for specimens,
/// including date interval grouping and specimen filters.
/// </summary>
internal class GenderGraph : IQueryReturningType<List<GraphModel>>
{
    /// <summary>
    /// Executes the gender graph query:
    /// applies date interval grouping, retrieves specimen filters, groups by gender and date,
    /// and joins with the listitem table to obtain label values.
    /// </summary>
    /// <param name="connect">
    /// An open NpgsqlConnection for executing the SQL query.
    /// </param>
    /// <param name="queryFilters">
    /// Filter configuration including date interval, specimen, and demographic parameters.
    /// </param>
    /// <returns>
    /// A list of GraphModel instances containing gender labels and corresponding counts
    /// over the specified date range.
    /// </returns>
    public async Task<List<GraphModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var dateSelector = new DateSelection();
        dateSelector.IncorporateDateInterval(queryFilters, "p.genderid", "li.value");

        _ = queryFilters.TryParseDateValue("startdate", out var startDate, new DateTime());
        _ = queryFilters.TryParseDateValue("enddate", out var endDate, new DateTime());

        var whereClause = await SpecimenFilter.GetWhereClauseAsync(queryFilters, connect);
        var join = await SpecimenFilter.GetJoinClauseAsync(queryFilters, connect);

        var sql = $"""
            with tablevalues as
                (
                    select p.genderId, count(*) as number, {dateSelector.Select}
                    from specimen s {join} {whereClause}
                    group by {dateSelector.GroupBy}
                )
            select {dateSelector.TableSelection} 
            from tablevalues tv
            inner join listitem li on tv.genderid = li.id
            order by {dateSelector.OrderBy};
            """;

        var result = await connect.QueryAsync<GraphModel>(sql, new { startDate, endDate });
        return [.. result];
    }
}

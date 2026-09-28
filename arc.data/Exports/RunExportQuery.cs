using arc.common;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports;

/// <summary>
/// Builds and executes an export query returning distinct specimen Ids based on
/// dynamic filters and joins constructed via <see cref="SpecimenFilter"/>.
/// </summary>
internal class RunExportQuery : IQueryReturningType<List<IdModel>>
{
    /// <summary>
    /// Executes the export specimen Id query using the supplied filter configuration.
    /// </summary>
    /// <param name="connect">Open database connection.</param>
    /// <param name="queryFilters">Filter bag controlling date range and other criteria.</param>
    /// <returns>List of distinct specimen Id models.</returns>
    public async Task<List<IdModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        _ = queryFilters.TryParseDateValue("startdate", out var startDate, new DateTime());
        _ = queryFilters.TryParseDateValue("enddate", out var endDate, new DateTime());

        var whereClause = await SpecimenFilter.GetWhereClauseAsync(queryFilters, connect);
        var join = await SpecimenFilter.GetJoinClauseAsync(queryFilters, connect);
        var with = "";

        // Create the organismId hierarchy

        var organismList = await SpecimenFilter.CreateOrganismHierarchyAsync(queryFilters, connect);
        if (!string.IsNullOrEmpty(organismList))
        {
            //with = "with culture as (select distinct specimenId from culture where specimenorganismid in (" + organismList + ")) ";
            //join += @"inner join culture cul on cul.SpecimenId = s.Id  ";
        }

        var sql = with + @"select distinct s.id from specimen s " + join + whereClause;

        var result = await connect.QueryAsync<IdModel>(sql, new { startDate, endDate });
        return result.ToList();
    }
}

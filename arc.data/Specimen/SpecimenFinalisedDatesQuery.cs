using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Returns the timestamp at which each of the given specimens entered the finalised state (534), taking the most
/// recent history row per specimen.
/// </summary>
internal class SpecimenFinalisedDatesQuery : IQueryReturningType<Dictionary<int, DateTime?>>
{
    private const int SpecimenFinalisedStateId = 534;

    /// <summary>
    /// Runs the query against the supplied connection.
    /// </summary>
    /// <param name="connect">An open connection to the database.</param>
    /// <param name="queryFilters">Filter configuration carrying the comma separated specimenids parameter.</param>
    /// <returns>A map of specimen id to finalised timestamp for the specimens that have one.</returns>
    public async Task<Dictionary<int, DateTime?>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var ids = SpecimenIdListParser.Parse(queryFilters.GetStringValue("specimenids"));
        if (ids.Length == 0)
        {
            return [];
        }

        var sql = """
            SELECT DISTINCT ON (SpecimenId) SpecimenId, LastModifiedDate
            FROM SpecimenStateHistory
            WHERE StateId = @StateId AND SpecimenId = ANY(@ids)
            ORDER BY SpecimenId, Id DESC
            """;

        var rows = await connect.QueryAsync<(int SpecimenId, DateTime? LastModifiedDate)>(
            sql, new { StateId = SpecimenFinalisedStateId, ids });

        return rows.ToDictionary(x => x.SpecimenId, x => x.LastModifiedDate);
    }
}

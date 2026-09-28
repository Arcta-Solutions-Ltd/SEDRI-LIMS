using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// For specimens whose current state is terminal, returns the timestamp at which they entered that state.
/// Used as the turnaround time end point on the archive list.
/// </summary>
internal class SpecimenTerminalStateEndTimesQuery : IQueryReturningType<Dictionary<int, DateTime?>>
{
    // Must match the terminal states filtered by SpecimenArchiveListQuery: 534 finalised, 528 rejected, 537 cancelled.
    private static readonly int[] TerminalStateIds = [534, 528, 537];

    /// <summary>
    /// Runs the query against the supplied connection.
    /// </summary>
    /// <param name="connect">An open connection to the database.</param>
    /// <param name="queryFilters">Filter configuration carrying the comma separated specimenids parameter.</param>
    /// <returns>A map of specimen id to the timestamp at which the terminal state was reached.</returns>
    public async Task<Dictionary<int, DateTime?>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var ids = SpecimenIdListParser.Parse(queryFilters.GetStringValue("specimenids"));
        if (ids.Length == 0)
        {
            return [];
        }

        var sql = """
            SELECT s.Id AS SpecimenId, sh.LastModifiedDate
            FROM specimen s
            INNER JOIN LATERAL (
                SELECT LastModifiedDate
                FROM SpecimenStateHistory
                WHERE SpecimenId = s.Id AND StateId = s.StateId
                ORDER BY Id DESC
                LIMIT 1
            ) sh ON true
            WHERE s.Id = ANY(@ids) AND s.StateId = ANY(@stateIds)
            """;

        var rows = await connect.QueryAsync<(int SpecimenId, DateTime? LastModifiedDate)>(
            sql, new { ids, stateIds = TerminalStateIds });

        return rows.ToDictionary(x => x.SpecimenId, x => x.LastModifiedDate);
    }
}

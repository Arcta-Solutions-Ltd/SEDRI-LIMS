using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace arc.data.Specimen;

/// <summary>
/// Returns the collection date of a specimen, keyed on the specimen id.
/// </summary>
internal class SpecimenCollectionDateQuery : IQueryReturningType<DateTime>
{
    /// <summary>
    /// Runs the query against the supplied connection.
    /// </summary>
    /// <param name="connect">An open connection to the database.</param>
    /// <param name="queryFilters">Filter configuration carrying the id parameter.</param>
    /// <returns>The date the specimen was collected.</returns>
    public async Task<DateTime> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.GetIntegerValue("id");

        var sql = "select s.collectiondate from specimen s where s.id = @id";

        return await connect.QueryFirstAsync<DateTime>(sql, new { id });
    }
}

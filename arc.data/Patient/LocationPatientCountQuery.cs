using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Patient;

/// <summary>
/// Represents a query for counting the number of patients associated with a specific location.
/// </summary>
internal class LocationPatientCountQuery : IQueryReturningInteger
{
    /// <summary>
    /// Executes the query asynchronously to count patients for a specified location ID.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="queryFilters">The query filters containing the location ID parameter.</param>
    /// <returns>
    /// A task representing the asynchronous operation. The task result contains the count of patients.
    /// </returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var locationId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

        var sql = "Select count(*) from patient where locationId = @LocationId";

        return await connect.QueryFirstAsync<int>(sql, new { locationId = int.Parse(locationId.Value) });
    }
}


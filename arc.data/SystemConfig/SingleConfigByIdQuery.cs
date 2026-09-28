using arc.data.model.Configuration;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig;

/// <summary>
/// Represents a database query to retrieve a single ConfigsDataModel by its ID.
/// Implements IQueryReturningType<T> for structured query execution.
/// </summary>
internal class SingleConfigByIdQuery : IQueryReturningType<ConfigsDataModel>
{
    /// <summary>
    /// Executes the query asynchronously to fetch a configuration record by ID.
    /// </summary>
    /// <param name="connect">Active PostgreSQL connection.</param>
    /// <param name="queryFilters">Query filter configuration containing parameters.</param>
    /// <returns>A ConfigsDataModel instance if found; otherwise, an empty model.</returns>
    public async Task<ConfigsDataModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

        var sql = @"SELECT c.Id, c.ConfigName, c.ConfigTypeId, c.contents FROM Configs c
                        WHERE c.Id = @Id
                        ORDER BY ConfigName";

        var resultList = await connect.QueryAsync<ConfigsDataModel>(sql, new { Id = int.Parse(id.Value) });

        var result = ! resultList.Any() ? new ConfigsDataModel() : resultList.First();
        return result;
    }
}

using arc.common.Models.SystemConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.SystemConfig;

/// <summary>
/// Represents a query to fetch a single configuration by name.
/// </summary>
internal class SingleConfigByNameQuery : IQueryReturningType<ConfigsModel>
{
    /// <summary>
    /// Executes the query to fetch a configuration by name asynchronously.
    /// </summary>
    /// <param name="connect">The NpgsqlConnection to the database.</param>
    /// <param name="queryFilters">The query filters containing the configuration name.</param>
    /// <returns>A task representing the asynchronous operation, with a ConfigsModel as the result.</returns>
    public async Task<ConfigsModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var configName = queryFilters.GetStringValue("configname")?.ToLower();

        var sql = @"select c.Id, c.ConfigName, c.ConfigTypeId, ct.Type, c.contents from Configs c
                        inner join ConfigType ct on c.ConfigTypeId = ct.Id
                        where c.ConfigName = @configName
                        order by ConfigName";

        var resultList = await connect.QueryAsync<ConfigsModel>(sql, new { configName });

        return !resultList.Any() ? new ConfigsModel() : resultList.First();
    }
}


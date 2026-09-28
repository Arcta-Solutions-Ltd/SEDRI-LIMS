using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Common;

/// <summary>
/// Gets all of the data from a table ordered by the id.
/// </summary>
/// <typeparam name="TClass">The type of item that represents a row in the database.</typeparam>
internal class GetAllQuery<TClass> : IQueryReturningType<List<TClass>>
{
    /// <summary>
    /// Executes the query to get all the data from a table.
    /// </summary>
    /// <param name="connect">Database connection</param>
    /// <param name="queryFilters">
    /// Query Filter Conditions:
    /// 1. tablename - the name of the table in the database you want to extract the rows from
    /// </param>
    /// <returns>All the rows in a table as a list</returns>
    public async Task<List<TClass>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var tableName = queryFilters.GetStringValue("tablename");

        var sql = $"select * from {tableName} order by id";

        var result = await connect.QueryAsync<TClass>(sql);

        return result.ToList();
    }
}

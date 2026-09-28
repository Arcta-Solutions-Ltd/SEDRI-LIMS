using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace arc.data.Common;
/// <summary>
/// A query class used to retrieve a single record by its ID from the specified table.
/// </summary>
/// <typeparam name="T">The type of the entity being queried.</typeparam>
internal class GetByIdQuery<T> : IQueryReturningType<T>
{
    /// <summary>
    /// Executes a query to fetch a single record by ID from a specified table.
    /// </summary>
    /// <param name="connect">The database connection to use.</param>
    /// <param name="entity">An instance of the entity type being queried.</param>
    /// <param name="queryFilters">Filter configuration containing the table name and ID.</param>
    /// <returns>A task representing the asynchronous operation, returning the queried entity.</returns>
    public async Task<T> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var tableName = queryFilters.GetStringValue("tablename");
        var id = queryFilters.GetIntegerValue("id");

        if (string.IsNullOrWhiteSpace(tableName))
        {
            throw new ArgumentException("Table name cannot be null or empty.", nameof(tableName));
        }

        if (id <= 0)
        {
            throw new ArgumentException("ID must be greater than zero.", nameof(id));
        }

        var sql = $"Select * from {tableName} where id = @Id";

        return await connect.QueryFirstOrDefaultAsync<T>(sql, new { id });
    }
}

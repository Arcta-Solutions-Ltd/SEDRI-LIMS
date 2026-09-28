using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Common;
/// <summary>
/// Gets the data for a single record.
/// </summary>
/// <typeparam name="T">The type of item that represents a row in the database.</typeparam>
internal class GetSingleWithMultipleParametersQuery<T> : IQuerySendingAndReturningType<T>
{
    /// <summary>
    /// Executes the query to get a single record from a table.
    /// </summary>
    /// <param name="connect">Database connection.</param>
    /// <param name="entity">Model to use to update the record.</param>
    /// <param name="queryFilters">
    /// Query Filter Conditions:
    /// Key-value pairs for the query conditions.
    /// </param>
    /// <returns>A task representing the asynchronous operation. The task result contains the data for the single record.</returns>
    public async Task<T> ExecuteAsync(NpgsqlConnection connect, T entity, QueryFilterConfig queryFilters)
    {
        var tableName = queryFilters.GetStringValue("tablename");

        var parameters = new DynamicParameters();

        var whereClauses = new List<string>();
        foreach (var parameter in queryFilters.Parameters)
        {
            if (entity.HasProperty(parameter.Key))
            {
                whereClauses.Add($"{parameter.Key} = @{parameter.Key}");
            } else
            {
                whereClauses.Add($"moredata->>{parameter.Key} = @{parameter.Key}");
            }
            parameters.Add(parameter.Key, parameter.Value);
        }

        string whereClause = string.Join(" AND ", whereClauses);

        string sql = $"SELECT * FROM {tableName} WHERE {whereClause}";

        return await connect.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }
}


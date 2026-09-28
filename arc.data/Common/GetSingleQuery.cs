using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace arc.data.Common;

/// <summary>
/// Gets the data for a single record.
/// </summary>
/// <typeparam name="T">The type of item that represents a row in the database.</typeparam>

internal class GetSingleQuery<T> : IQuerySendingAndReturningType<T>
{
    /// <summary>
    /// Executes the query to get all a single record from a table.
    /// </summary>
    /// <param name="connect">Database connection</param>
    /// <param name="queryFilters">
    /// Query Filter Conditions:
    /// 1. tablename - the name of the table in the database you want to extract the rows from
    /// </param>
    /// <returns>First record that matches the query</returns>
    /// <exception cref="ArgumentException">When required comparision value for a field stored in the moredata column is not found</exception>
    public async Task<T> ExecuteAsync(NpgsqlConnection connect, T entity, QueryFilterConfig queryFilters)
    {
        var tableName = queryFilters.GetStringValue("tablename");
        var keyColumn = queryFilters.GetStringValue("keycolumn");
        var queryFilterHasMoreDataValue = queryFilters.TryGetStringValue("moredatavalue", out var moreDataValue);

        var (sql,parameters) = CreateSqlStringWithParameter(entity, tableName, keyColumn, queryFilterHasMoreDataValue, moreDataValue);

        return await connect.QueryFirstOrDefaultAsync<T>(sql, parameters);
    }

    internal (string, DynamicParameters) CreateSqlStringWithParameter(object entity, string tableName, string keyColumn, bool queryFilterHasMoreDataValue, string moreDataValue)
    {
        var parameters = new DynamicParameters();
        parameters.Add("tableName", tableName);

        string sql;
        if (entity.HasProperty(keyColumn))
        {
            var keyColumnValue = entity.GetPropertyValue(keyColumn);
            sql = $"Select * from {tableName} where {keyColumn} = @{keyColumn}";
            parameters.Add(keyColumn, keyColumnValue);
        }
        else
        {
            if (queryFilterHasMoreDataValue)
            {
                parameters.Add(keyColumn, moreDataValue);
                sql = $"Select * from {tableName} where moredata->>'{keyColumn}' = @{keyColumn}";
            }
            else
            {
                throw new ArgumentException($"No value to compare against the moredata value found for parameter {keyColumn}");
            }
        }
        return (sql,parameters);
    }
}



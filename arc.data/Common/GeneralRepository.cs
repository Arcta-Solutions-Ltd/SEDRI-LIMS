using arc.app.Common;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Common;

/// <summary>
/// General repository containing generic methods for accessing a database. Other repositories will normally inherit from this.
/// </summary>
public class GeneralRepository : IGeneralRepository
{
    protected readonly ISqlQuery _sqlQuery;
    protected readonly ILogWriter _logWriter;
    protected readonly ISqlCommand _sqlCommand;

    public GeneralRepository(ISqlQuery sqlQuery, ILogWriter logWriter, ISqlCommand sqlCommand)
    {
        _sqlQuery = sqlQuery;
        _logWriter = logWriter;
        _sqlCommand = sqlCommand;
    }

    /// <summary>
    /// Calls the query the will return all the rows in a table in the database.
    /// </summary>
    /// <param name="tableName">The name of the table in the database you want to extract the rows from</param>
    /// <typeparam name="T">The type of item that represents a row in the database.</typeparam>
    /// <returns>IEnumerable of all the rows in a table in the database</returns>
    public async Task<IEnumerable<T>> GetAllAsync<T>(string tableName)
    {
        _logWriter.LogInfo("Run get all query", "GeneralRepository", "GetAllAsync");

        var queryFilters = new QueryFilterConfig();
        queryFilters.AddString("tablename", tableName);
        return await _sqlQuery.QueryReturningTypeAsync(new GetAllQuery<T>(), "Get all", queryFilters);
    }

    /// <summary>
    /// Retrieves a single record by its ID from the specified table.
    /// </summary>
    /// <typeparam name="T">The type of the entity to retrieve.</typeparam>
    /// <param name="entity">An instance of the entity used as a template for the query.</param>
    /// <param name="tableName">The name of the database table to query.</param>
    /// <param name="id">The unique identifier of the record to retrieve.</param>
    /// <returns>A task representing the asynchronous operation, containing the retrieved entity.</returns>
    public async Task<T> GetByIdAsync<T>(string tableName, int id) where T : new()
    {
        _logWriter.LogInfo("Run get single record query", "GeneralRepository", "GetSingleRecordAsync");

        var queryFilters = new QueryFilterConfig().AddInteger("id", id);
        queryFilters.AddString("tablename", tableName);

        return await _sqlQuery.QueryReturningTypeAsync(new GetByIdQuery<T>(), "Get single record by id", queryFilters);
    }

    /// <summary>
    /// Calls the query the will return a single row of a table in the database.
    /// </summary>
    /// <param name="entity">Model to use to update the record</param>
    /// <param name="tableName">Name of the table to update</param>
    /// <param name="keyColumn">Name of the property to select the field on</param>
    /// <param name="moredataValue">Value of the field to use in the sql expression if it is stored in Moredata</param>
    /// <typeparam name="T">The type of item that represents a row in the database.</typeparam>
    /// <returns>Object representing the row in the database</returns>
    public async Task<T> GetSingleRecordAsync<T>(T entity, string tableName, string keyColumn, string moredataValue = "") where T : new()
    {
        _logWriter.LogInfo("Run get single record query", "GeneralRepository", "GetSingleRecordAsync");

        var queryFilters = new QueryFilterConfig();
        queryFilters.AddString("tablename", tableName);
        queryFilters.AddString("keycolumn", keyColumn);
        queryFilters.AddString("moredatavalue", moredataValue);
        return await _sqlQuery.QuerySendingAndReturningTypeAsync(new GetSingleQuery<T>(), "Get single record", entity, queryFilters);
    }

    /// <summary>

    /// Calls the query that will return a single row of a table in the database.
    /// </summary>
    /// <param name="entity">Model to use to update the record</param>
    /// <param name="tableName">Name of the table to update</param>
    /// <param name="keyValues">List of parameters for the query where Key holds the fields name and Value the value to find in the field</param>
    /// <typeparam name="T">The type of item that represents a row in the database.</typeparam>
    /// <returns>Object representing the row in the database</returns>
    public async Task<T> GetSingleRecordWithMultipleParametersAsync<T>(T entity, string tableName, List<KeyValueModel> keyValues) where T : new()
    {
        _logWriter.LogInfo("Run get single record query", "GeneralRepository", "GetSingleRecordWithMultipleParametersAsync");

        var queryFilters = new QueryFilterConfig();
        queryFilters.AddString("tablename", tableName);

        foreach (var keyValue in keyValues)
        {
            queryFilters.AddString(keyValue.Key, keyValue.Value);
        }

        return await _sqlQuery.QuerySendingAndReturningTypeAsync(new GetSingleWithMultipleParametersQuery<T>(), "Get single record with parameters", entity, queryFilters);
    }

    /// <summary>

    /// Adds a record to the database
    /// </summary>
    /// <param name="entity">Model to use to update the record</param>
    /// <param name="tableName">Name of the table to update</param>
    /// <returns>Number of rows affected in the database</returns>
    public async Task<int> AddAsync<T>(T entity, string tableName)
    {
        _logWriter.LogInfo("Add record to database", "GeneralRepository", "AddAsync");

        var result =  await _sqlCommand.CommandWithTypeAndStringParametersAsync(new AddCommand<T>(), "Add record", entity, tableName);
        return result;
    }

    /// <summary>
    /// Updates a record in the database.
    /// </summary>
    /// <param name="entity">Model to use to update the record</param>
    /// <param name="tableName">Name of the table to update</param>
    /// <param name="keyColumn">Name of the property to select the field on</param>
    /// <returns>Number of rows affected in the database</returns>
    public async Task<int> UpdateAsync<T>(T entity, string tableName, string keyColumn)
    {
        _logWriter.LogInfo("Run update command", "GeneralRepository", "UpdateAsync");

        return await _sqlCommand.CommandWithTypeAndStringParametersAsync(new UpdateCommand<T>(), "Update record", entity, tableName, keyColumn );
    }
}





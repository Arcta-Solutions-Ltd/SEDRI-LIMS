using arc.app;
using arc.app.Common;
using arc.common;
using arc.common.Data;
using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.common.Utils;
using arc.data.Configuration;
using arc.data.Instruments;
using arc.data.Utils;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace arc.data
{
    /// <summary>
    /// Repository for generic SQL operations.
    /// </summary>
    public class SqlGenericRepository : IGenericRepository, ILoginRepository
    {
        private string _config;
        private TokenInfoModel _token;
        private readonly IOptionsMonitor<DataOptions> _options;
        private readonly ILogWriter _logWriter;
        private readonly IGenerateMoreData _moreDataGenerator;
        private readonly IMoreDataRepository _moreDataRepository;
        private readonly IJsonElementRemover _jsonElementRemover;
        private readonly IInstrumentInterfaceHandler _instrumentInterfaceHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlGenericRepository"/> class.
        /// </summary>
        /// <param name="options">The data options monitor.</param>
        /// <param name="logWriter">The log writer.</param>
        /// <param name="moreDataGenerator">The more data generator.</param>
        /// <param name="moreDataRepository">The more data repository.</param>
        /// <param name="jsonElementRemover">The JSON element remover.</param>
        /// <param name="instrumentInterfaceHandler">The instrument interface handler.</param>
        public SqlGenericRepository(IOptionsMonitor<DataOptions> options, ILogWriter logWriter, IGenerateMoreData moreDataGenerator, IMoreDataRepository moreDataRepository,
                                    IJsonElementRemover jsonElementRemover, IInstrumentInterfaceHandler instrumentInterfaceHandler)
        {
            _options = options;
            _logWriter = logWriter;
            _moreDataGenerator = moreDataGenerator;
            _moreDataRepository = moreDataRepository;
            _jsonElementRemover = jsonElementRemover;
            _instrumentInterfaceHandler = instrumentInterfaceHandler;
        }

        /// <summary>
        /// Adds the configuration string.
        /// </summary>
        /// <param name="config">The configuration string.</param>
        public void AddConfiguration(string config)
        {
            _config = config;
        }

        /// <summary>
        /// Adds the token information model.
        /// </summary>
        /// <param name="token">The token information model.</param>
        public void AddToken(TokenInfoModel token)
        {
            _token = token;
        }

        /// <summary>
        /// Asynchronously adds a new record to the database within a transaction scope.
        /// </summary>
        /// <param name="dataToSave">The data to save as a JSON string.</param>
        /// <param name="command">Optional event model command.</param>
        /// <param name="stringFields">Optional list of string fields.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the new record.</returns>
        public async Task<int> AddAsync(string dataToSave, EventModel command = null, string stringFields = "")
        {
            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var returnId = await AddWithoutScopeAsync(dataToSave, command, stringFields);

                scope.Complete();
                return returnId;
            }
            catch (TransactionAbortedException ex)
            {
                _logWriter.LogError($"Create Generic Record Transaction aborted: {ex.Message}", nameof(SqlGenericRepository), nameof(AddAsync));
                throw new Exception(ex.Message);
            }
            catch (Exception e)
            {
                _logWriter.LogError($"Updating record aborted with message: {e.Message}", nameof(SqlGenericRepository), nameof(AddAsync));
                throw;
            }
        }

        /// <summary>
        /// Asynchronously adds a new record to the database without a transaction scope.
        /// Uses parameterized queries to prevent SQL injection attacks.
        /// </summary>
        /// <param name="dataToSave">The data to save as a JSON string.</param>
        /// <param name="command">Optional event model command.</param>
        /// <param name="stringFields">Optional list of string fields.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the ID of the new record.</returns>
        public async Task<int> AddWithoutScopeAsync(string dataToSave, EventModel command = null, string stringFields = "")
        {
            SqlBuildResult sqlResult = null;

            try
            {
                var moreData = _moreDataGenerator.GetMoreDataJsonString(_config, dataToSave);
                if (moreData != "{}")
                {
                    _logWriter.LogInfo("More data found and being combined", nameof(SqlGenericRepository), nameof(AddAsync));
                    dataToSave = _moreDataGenerator.GetLeftOverData();
                }

                using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);

                var builder = new SqlBuilder(dataToSave, stringFields);
                _logWriter.LogInfo("Start building the parameterized SQL", nameof(SqlGenericRepository), nameof(AddAsync));
                sqlResult = builder.BuildInsertSqlParameterized(_config, moreData);
                LogExcludedMetadataFields(sqlResult, nameof(AddAsync));

                _logWriter.LogInfo("Run the SQL insert statement with parameters", nameof(SqlGenericRepository), nameof(AddAsync));
                var returnValue = connect.QueryFirst(sqlResult.Sql, sqlResult.Parameters);
                var returnId = (int)returnValue.id;

                if (command != null)
                {
                    _logWriter.LogInfo("Update the state of the record", nameof(SqlGenericRepository), nameof(AddAsync));
                    await UpdateStateAsync(connect, command);
                }

                return returnId;
            }
            catch (Exception e)
            {
                _logWriter.LogError($"Updating record aborted with message: {e.Message}{DescribeFailedStatement(sqlResult)}", nameof(SqlGenericRepository), nameof(AddAsync));
                throw;
            }
        }

        /// <summary>
        /// Writes a warning entry naming any payload keys dropped because they are client metadata
        /// rather than columns on the target table. Present so a failed or unexpectedly incomplete
        /// save can be diagnosed on an installed system where the debugger is unavailable.
        /// </summary>
        /// <param name="sqlResult">The build result to inspect.</param>
        /// <param name="methodName">Calling method name used for the log entry.</param>
        private void LogExcludedMetadataFields(SqlBuildResult sqlResult, string methodName)
        {
            if (sqlResult.ExcludedMetadataFields.Count == 0)
            {
                return;
            }

            var excluded = string.Join(", ", sqlResult.ExcludedMetadataFields);
            _logWriter.LogInfo($"WARN: Excluded client metadata fields from {_config} SQL: {excluded}", nameof(SqlGenericRepository), methodName);
        }

        /// <summary>
        /// Describes a failed statement for the error log using the table name, the SQL template and the
        /// parameter names only. Parameter values are deliberately omitted because payloads carry patient data.
        /// </summary>
        /// <param name="sqlResult">The build result to describe, which may be null when the failure happened before the build.</param>
        /// <returns>A log fragment describing the statement, or an empty string when nothing was built.</returns>
        private string DescribeFailedStatement(SqlBuildResult sqlResult)
        {
            if (sqlResult == null || string.IsNullOrEmpty(sqlResult.Sql))
            {
                return $" Table: {_config}. No SQL statement was built.";
            }

            var parameterNames = string.Join(", ", sqlResult.Parameters.Keys);
            return $" Table: {_config}. SQL: {sqlResult.Sql}. Parameters: {parameterNames}";
        }

        /// <summary>
        /// Asynchronously retrieves the first value from the database configuration.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. The task result contains the first value as a JSON string.</returns>
        public async Task<string> GetFirstValueAsync()
        {
            await using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
            var sql = $"select * from {_config}";

            var result = await connect.QueryAsync(sql);

            return result.Any() ? JsonConvert.SerializeObject(result.First()) : string.Empty;
        }

        /// <summary>
        /// Asynchronously retrieves a single value from the database based on the specified field and value.
        /// </summary>
        /// <param name="field">The field to filter by.</param>
        /// <param name="value">The value to filter by.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the single value as a JSON string.</returns>
        public async Task<string> GetSingleValueAsync(string field, string value)
        {
            await using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
            var sql = $"select * from {_config} where {field} = @Value";

            var result = await connect.QueryAsync(sql, new { Value = value });

            return result.Any() ? JsonConvert.SerializeObject(result.First()) : string.Empty;
        }

        /// <summary>
        /// Asynchronously retrieves a single record from the database based on the specified query configuration and filters.
        /// </summary>
        /// <param name="queryConfig">The query configuration.</param>
        /// <param name="filters">Optional query filters.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the single record as a JSON string.</returns>
        public async Task<string> GetSingleAsync(QueryConfig queryConfig, QueryFilterConfig filters = null)
        {
            try
            {
                await using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
                var sql = filters == null ? queryConfig.BuildQuery(_token) : queryConfig.BuildQuery(_token, filters);

                _logWriter.LogInfo("Executing query", nameof(SqlGenericRepository), nameof(GetSingleAsync));
                var result = await connect.QueryAsync(sql);

                if (!result.Any())
                {
                    _logWriter.LogInfo("No record found", nameof(SqlGenericRepository), nameof(GetSingleAsync));
                    return string.Empty;
                }

                return JsonConvert.SerializeObject(result.First());
            }
            catch (Exception e)
            {
                _logWriter.LogError($"Getting a single record aborted with message: {e.Message}", nameof(SqlGenericRepository), nameof(GetSingleAsync));
                throw;
            }
        }

        /// <summary>
        /// Asynchronously edits an existing record in the database.
        /// </summary>
        /// <param name="dataToSave">The data to save as a JSON string.</param>
        /// <param name="Id">The ID of the record to update.</param>
        /// <param name="command">Optional event model command.</param>
        /// <param name="stringFields">Optional list of string fields.</param>
        public async Task EditAsync(string dataToSave, string Id, EventModel command = null, string stringFields = "")
        {
            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                await using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
                await connect.OpenAsync();
                await EditWithoutScopeAsync(dataToSave, Id, command, stringFields, connect);

                scope.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                _logWriter.LogError($"Create Generic Record Transaction aborted: {ex.Message}", nameof(SqlGenericRepository), nameof(EditAsync));
                throw new Exception(ex.Message);
            }
            catch (Exception e)
            {
                _logWriter.LogError($"Updating record aborted with message: {e.Message}", nameof(SqlGenericRepository), nameof(EditAsync));
                throw;
            }
        }

        /// <summary>
        /// Performs an update without an ambient transaction scope, merges supplemental JSON payload if present,
        /// and applies any post‐update event logic (state changes or pending instrument creation).
        /// Uses parameterized queries to prevent SQL injection attacks.
        /// </summary>
        /// <param name="dataToSave">The initial JSON payload to persist; may be overridden by leftover data after merging.</param>
        /// <param name="Id">The identifier of the record to update.</param>
        /// <param name="command">An EventModel describing post‐update actions; pass null to skip event handling.</param>
        /// <param name="stringFields">Comma‐separated list of string field names requiring special quoting or escaping.</param>
        /// <param name="connect">Optional open connection; when null a new connection is opened and disposed.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentException">Thrown when the Id is not a valid integer.</exception>
        /// <exception cref="Exception">Thrown when the update transaction is aborted or any other error occurs.</exception>
        public async Task EditWithoutScopeAsync(string dataToSave, string Id, EventModel command, string stringFields, NpgsqlConnection connect = null)
        {
            SqlBuildResult sqlResult = null;
            var ownsConnection = connect == null;

            try
            {
                if (ownsConnection)
                {
                    connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
                    await connect.OpenAsync();
                }

                var moreData = _moreDataGenerator.GetMoreDataJsonString(_config, dataToSave);
                if (moreData != "{}")
                {
                    _logWriter.LogInfo("More data found and being combined", nameof(SqlGenericRepository), nameof(EditWithoutScopeAsync));
                    moreData = await _moreDataRepository.CombineWithExistingFieldAsync(moreData, _config, Id, connect);
                }

                var leftOverData = _moreDataGenerator.GetLeftOverData();
                if (leftOverData != "{}")
                {
                    dataToSave = leftOverData;
                }

                var builder = new SqlBuilder(dataToSave, stringFields);
                _logWriter.LogInfo("Start building the parameterized SQL", nameof(SqlGenericRepository), nameof(EditWithoutScopeAsync));
                sqlResult = builder.BuildUpdateSqlParameterized(_config, Id, moreData);
                LogExcludedMetadataFields(sqlResult, nameof(EditWithoutScopeAsync));

                if (!string.IsNullOrEmpty(sqlResult.Sql))
                {
                    _logWriter.LogInfo("Execute the SQL statement with parameters", nameof(SqlGenericRepository), nameof(EditWithoutScopeAsync));
                    await connect.ExecuteAsync(sqlResult.Sql, sqlResult.Parameters);
                }

                if (command != null)
                {
                    _logWriter.LogInfo("Update the state of the record", nameof(SqlGenericRepository), nameof(EditWithoutScopeAsync));
                    await UpdateStateAsync(connect, command);

                    if (command.Event.Equals("editculture", StringComparison.OrdinalIgnoreCase))
                    {
                        await _instrumentInterfaceHandler.CreateInstrumentPendingResultsWhenEditingACulture(dataToSave, connect);
                    }
                }
            }
            catch (TransactionAbortedException ex)
            {
                _logWriter.LogError($"Updating Generic Record Transaction aborted: {ex.Message}{DescribeFailedStatement(sqlResult)}", nameof(SqlGenericRepository), nameof(EditWithoutScopeAsync));
                throw new Exception(ex.Message);
            }
            catch (ArgumentException ex)
            {
                _logWriter.LogError($"Invalid Id parameter: {ex.Message}", nameof(SqlGenericRepository), nameof(EditWithoutScopeAsync));
                throw;
            }
            catch (Exception e)
            {
                _logWriter.LogError($"Updating record aborted with message: {e.Message}{DescribeFailedStatement(sqlResult)}", nameof(SqlGenericRepository), nameof(EditWithoutScopeAsync));
                throw;
            }
            finally
            {
                if (ownsConnection && connect != null)
                {
                    await connect.DisposeAsync();
                }
            }
        }

        /// <summary>
        /// Asynchronously retrieves a list of records from the database based on the specified query configuration and filters.
        /// </summary>
        /// <param name="queryConfig">The query configuration.</param>
        /// <param name="filters">Optional query filters.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the list of records as a JSON string.</returns>
        public async Task<string> GetListAsync(QueryConfig queryConfig, QueryFilterConfig filters = null)
        {
            try
            {
                await using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);

                _logWriter.LogInfo("Build the query SQL", nameof(SqlGenericRepository), nameof(GetListAsync));
                var sql = queryConfig.BuildQuery(_token, filters);

                _logWriter.LogInfo("Run the query", nameof(SqlGenericRepository), nameof(GetListAsync));
                var result = await connect.QueryAsync(sql);

                return JsonConvert.SerializeObject(result);
            }
            catch (Exception e)
            {
                var orderBy = filters?.OrderBy ?? string.Empty;
                var orderDescending = filters?.OrderDescending ?? false;
                _logWriter.LogError(
                    $"Selecting record aborted for query {queryConfig.Query}, OrderBy={orderBy}, OrderDescending={orderDescending}: {e.Message}",
                    nameof(SqlGenericRepository),
                    nameof(GetListAsync));
                throw;
            }
        }


        /// <summary>
        /// Asynchronously deletes a record from the database based on the specified ID and command.
        /// </summary>
        /// <param name="id">The ID of the record to delete.</param>
        /// <param name="command">The event model command.</param>
        public async Task DeleteAsync(string id, EventModel command)
        {
            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                {
                    var sql = "delete from " + _config + " where Id = @Id";

                    var rowsAffected = await connect.ExecuteAsync(sql, new { Id = int.Parse(id) });
                    if (rowsAffected == 0 && TestTableExtensions.IsIdempotentTestDeleteTable(_config))
                    {
                        _logWriter.LogInfo(
                            $"Idempotent delete: no row for id {id} in {_config}",
                            nameof(SqlGenericRepository),
                            nameof(DeleteAsync));
                    }
                    else
                    {
                        await UpdateStateAsync(connect, command);
                    }
                }
                scope.Complete();
            }
            catch (Exception e)
            {
                _logWriter.LogError($"Updating record aborted with message: {e.Message}", nameof(SqlGenericRepository), nameof(DeleteAsync));
                throw;
            }
        }

        /// <summary>
        /// Retrieves the count of records from the database based on the specified query configuration and filters.
        /// </summary>
        /// <param name="queryConfig">The query configuration.</param>
        /// <param name="filters">Optional query filters.</param>
        /// <param name="excludeTokenFilters">Whether to exclude token filters.</param>
        /// <returns>The count of records.</returns>
        public long GetCountAsync(QueryConfig queryConfig, QueryFilterConfig filters = null, bool excludeTokenFilters = false)
        {
            using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);

            var sql = queryConfig.BuildQuery(_token, filters, excludeTokenFilters);

            var result = connect.QueryFirst<long>(sql);

            return result;
        }

        /// <summary>
        /// Asynchronously updates the state of the specified event model.
        /// </summary>
        /// <param
        public async Task UpdateStateAsync(EventModel model)
        {
            using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
            await UpdateStateAsync(connect, model);
        }

        /// <summary>
        /// Asynchronously updates the state of a record in the database.
        /// </summary>
        /// <param name="connect">The Npgsql connection to use for the update.</param>
        /// <param name="model">The event model containing the update information.</param>
        private async Task UpdateStateAsync(NpgsqlConnection connect, EventModel model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.Table) && !string.IsNullOrEmpty(model.Field) && !string.IsNullOrEmpty(model.NewStateId))
                {
                    string id = model.StateTargetId ?? model.Id;
                    var sql = @"update " + model.Table + " set " + model.Field + " = @StateId, lastmodifieddate = now() where id = @Id";
                    await connect.ExecuteAsync(sql, new { StateId = int.Parse(model.NewStateId), Id = int.Parse(id) });

                    sql = @"insert into SpecimenStateHistory(StateId, SpecimenId, LastModifiedDate) values(@StateId, @SpecimenId, now())";
                    await connect.ExecuteAsync(sql, new { StateId = int.Parse(model.NewStateId), SpecimenId = int.Parse(id) });
                }
            }
            catch (Exception e)
            {
                _logWriter.LogError($"Update State SQL execution error: {e.Message}", nameof(SqlGenericRepository), nameof(UpdateStateAsync));
                throw;
            }
        }

    }
}

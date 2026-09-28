//using arc.data.Configuration;
//using arc.domain.Configuration.QueryFiltersConfig;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using Npgsql;
//using System;
//using System.Threading.Tasks;

//namespace arc.data
//{
//    public class SqlQuery : ISqlQuery
//    {
//        private readonly IOptionsMonitor<DataOptions> _options;
//        private readonly ILogger _logger;

//        public SqlQuery(IOptionsMonitor<DataOptions> options, ILogger logger)
//        {
//            _options = options;
//            _logger = logger;
//        }

//        public async Task<int> QueryReturningIntegerAsync(IQueryReturningInteger query, string errorMessage, QueryFilterConfig queryFilters)
//        {
//            var id = 0;

//            try
//            {
//                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
//                {
//                    id = await query.ExecuteAsync(connect, queryFilters);
//                }
//            }
//            catch (Exception e)
//            {
//                _logger.LogError(errorMessage + " sql execution error : {0}", e.Message);
//            }
//            return id;
//        }

//        public async Task<string> QueryReturningStringAsync(IQueryReturningString query, string errorMessage, QueryFilterConfig queryFilters)
//        {
//            var retVal = "";

//            try
//            {
//                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
//                {
//                    retVal = await query.ExecuteAsync(connect, queryFilters);
//                }
//            }
//            catch (Exception e)
//            {
//                _logger.LogError(errorMessage + " sql execution error : {0}", e.Message);
//            }
//            return retVal;
//        }

//        public async Task<T> QueryReturningTypeAsync<T>(IQueryReturningType<T> query, string errorMessage, QueryFilterConfig queryFilters) where T : new()
//        {
//            var ret = new T();

//            try
//            {
//                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
//                {
//                    ret = await query.ExecuteAsync(connect, queryFilters);
//                }
//            }
//            catch (Exception e)
//            {
//                _logger.LogError(errorMessage + " sql execution error : {0}", e.Message);
//            }
//            return ret;
//        }

//        public string QueryReturningString(IQueryReturningStringNotAsync query, string errorMessage, QueryFilterConfig queryFilters)
//        {
//            var retVal = "";

//            try
//            {
//                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
//                {
//                    retVal = query.Execute(connect, queryFilters);
//                }
//            }
//            catch (Exception e)
//            {
//                _logger.LogError(errorMessage + " sql execution error : {0}", e.Message);
//            }
//            return retVal;
//        }
//    }
//}

using arc.data.Configuration;
using arc.domain.Configuration.QueryFiltersConfig;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace arc.data
{
    /// <summary>
    /// Used to wrap a sql query with a connection in a common way.
    /// </summary>
    public class SqlQuery(IOptionsMonitor<DataOptions> options, ILogger logger) : ISqlQuery
    {
        private readonly IOptionsMonitor<DataOptions> _options = options;
        private readonly ILogger _logger = logger;

        /// <summary>
        /// Carries out a query against the database returning an integer.
        /// </summary>
        /// <param name="query">The method which represents the query to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the query fails</param>
        /// <param name="queryFilters">Set of filter conditions which are passed to the query</param>
        /// <returns>Returns an integer</returns>
        public async Task<int> QueryReturningIntegerAsync(IQueryReturningInteger query, string errorMessage, QueryFilterConfig queryFilters)
        {
            return await ExecuteQueryAsync(async (conn) => await query.ExecuteAsync(conn, queryFilters), errorMessage);
        }

        /// <summary>
        /// Carries out a query against the database returning a string.
        /// </summary>
        /// <param name="query">The method which represents the query to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the query fails</param>
        /// <param name="queryFilters">Set of filter conditions which are passed to the query</param>
        /// <returns>Returns an string</returns>
        public async Task<string> QueryReturningStringAsync(IQueryReturningString query, string errorMessage, QueryFilterConfig queryFilters)
        {
            return await ExecuteQueryAsync(async (conn) => await query.ExecuteAsync(conn, queryFilters), errorMessage);
        }

        /// <summary>
        /// Carries out a query against the database returning a type.
        /// </summary>
        /// <param name="query">The method which represents the query to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the query fails</param>
        /// <param name="queryFilters">Set of filter conditions which are passed to the query</param>
        /// <returns>Returns an instance of a type</returns>
        public async Task<T> QueryReturningTypeAsync<T>(IQueryReturningType<T> query, string errorMessage, QueryFilterConfig queryFilters) where T : new()
        {
            return await ExecuteQueryAsync(async (conn) => await query.ExecuteAsync(conn, queryFilters), errorMessage);
        }

        /// <summary>
        /// Carries out a query against the database returning a type.
        /// </summary>
        /// <param name="query">The method which represents the query to be carried out</param>
        /// <param name="errorMessage">The error message to be displayed if the query fails</param>
        /// <param name="entity">Instance of the type, normally reprsenting the model for the table</param>
        /// <param name="queryFilters">Set of filter conditions which are passed to the query</param>
        /// <returns>Returns an instance of a type representing the results of the query</returns>
        public async Task<T> QuerySendingAndReturningTypeAsync<T>(IQuerySendingAndReturningType<T> query, string errorMessage, T entity, QueryFilterConfig queryFilters) where T : new()
        {
            return await ExecuteQueryAsync(async (conn) => await query.ExecuteAsync(conn, entity, queryFilters), errorMessage);
        }

        private async Task<TResult> ExecuteQueryAsync<TResult>(Func<NpgsqlConnection, Task<TResult>> query, string errorMessage)
        {
            try
            {
                using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
                return await query(connect);
            }
            catch (Exception e)
            {
                _logger.LogError($"{errorMessage} SQL execution error: {e.Message}");
                throw;
            }
        }
    }
}


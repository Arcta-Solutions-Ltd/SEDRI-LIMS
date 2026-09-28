using arc.app.Common;
using arc.domain.Configuration.QueryConfig;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace arc.data.Configuration
{
    public class QueryConfigRepository : IRepository<QueryConfig>
    {
        private readonly IOptionsMonitor<DataOptions> _options;
        private readonly ILogger _logger;

        public QueryConfigRepository(IOptionsMonitor<DataOptions> options, ILogger logger)
        {
            _options = options;
            _logger = logger;
        }

        public async Task AddAsync(QueryConfig record)
        {
            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                {
                    var sql = "insert into Query(Name, TableName, Fields) values(@Query,@TableName,@Fields) ";
                    var result = await connect.ExecuteAsync(sql, record);
                }

                scope.Complete();
            }
            catch (TransactionAbortedException ex)
            {
                _logger.LogError("Create Event Transaction aborted : {0}", ex.Message);
            }
        }

        public async Task<QueryConfig> GetSingleAsync(string queryName)
        {
            try
            {
                using var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);

                _logger.LogInformation("Getting the confguration for query " + queryName);

                var sql = "select cast(q.Id As varchar(20)) As Id, q.Name As Query, q.TableName, q.Fields from Query q " +
                          "where q.Name = '" + queryName + "'";
                var queryResult = await connect.QueryAsync<QueryConfig>(sql);
                var result = queryResult.FirstOrDefault();

                _logger.LogInformation("Configuration information for query " + queryName + " successfully retrieved");

                return result;
            }
            catch (NpgsqlException eSql)
            {
                _logger.LogError("Error getting configuration for query " + queryName + " Error: " + eSql.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError("Error getting configuration for query " + queryName + " Error: " + e.Message);
                throw;
            }
        }
    }
}

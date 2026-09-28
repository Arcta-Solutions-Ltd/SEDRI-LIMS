using arc.app.Common;
using arc.data.Configuration;
using arc.domain.Configuration.EventsConfig;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace arc.data.EventData
{
    public class EventRepository : IRepository<EventConfig>
    {
        private readonly IOptionsMonitor<DataOptions> _options;
        private readonly ILogger _logger;

        public EventRepository(IOptionsMonitor<DataOptions> options, ILogger logger)
        {
            _options = options;
            _logger = logger;
        }

        public async Task AddAsync(EventConfig record)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                    {
                        // Check whether topic exists and add if not

                        var sql = "select Id from Topic Where Name = @TopicName";
                        var topicId = await connect.QueryFirstAsync<int>(sql, record);

                        if (topicId == 0)
                        {
                            sql = "Insert into Topic(Name) values(@TopicName) " +
                                  "returning Id";
                            topicId = await connect.QueryFirstAsync<int>(sql, record);

                        }

                        var p = new { TopicId = topicId, record.EventName, record.TableName, record.EventType };

                        // Add the event
                        sql = "insert into Event(TopicId, Name, TableName, Type) values(@TopicId,@EventName,@TableName,@EventType) ";
                        var result = await connect.ExecuteAsync(sql, p);
                    }

                    scope.Complete();
                }
            }
            catch (TransactionAbortedException ex)
            {
                _logger.LogError("Create Event Transaction aborted : {0}", ex.Message);
            }
        }

        public async Task<EventConfig> GetSingleAsync(string eventName)
        {
            try
            {
                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                {


                    _logger.LogInformation("Getting the confguration for event " + eventName);

                    var sql = "select cast(ev.Id As varchar(30)) As Id, tp.Name As TopicName, ev.Name As EventName, " +
                              "ev.Type As EventType, ev.TableName from Event ev " +
                              "inner join Topic tp on tp.Id = ev.TopicId " +
                              "where ev.Name = '" + eventName + "'";
                    var queryResult = await connect.QueryAsync<EventConfig>(sql);
                    var result = queryResult.FirstOrDefault();

                    _logger.LogInformation("Configuration information for event " + eventName + " successfully retrieved");

                    return result;
                };
            }
            catch (NpgsqlException eSql)
            {
                _logger.LogError("Error getting configuration for event " + eventName + " Error: " + eSql.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError("Error getting configuration for event " + eventName + " Error: " + e.Message);
                throw;
            }
        }
    }
}

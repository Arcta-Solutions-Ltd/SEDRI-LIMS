using arc.app.Common;
using arc.common;
using arc.common.Models;
using arc.common.Models.Specimen;
using arc.data.Configuration;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.EventData
{
    /// <summary>
    /// Repository for Queue table operations. Inserts include a hash chain value for tamper detection.
    /// </summary>
    public class MessageRepository : IMessageRepository
    {
        private readonly IOptionsMonitor<DataOptions> _options;
        private readonly ILogger _logger;
        private readonly IQueueHashService _queueHashService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRepository"/> class.
        /// </summary>
        /// <param name="options">Data options containing the connection string.</param>
        /// <param name="logger">Logger for error reporting.</param>
        /// <param name="queueHashService">Service for computing the hash chain value.</param>
        public MessageRepository(IOptionsMonitor<DataOptions> options, ILogger logger, IQueueHashService queueHashService)
        {
            _options = options;
            _logger = logger;
            _queueHashService = queueHashService;
        }

        /// <summary>
        /// Adds a new message to the Queue table with a hash chain value for tamper detection.
        /// The hash is computed from the previous record's Message and the current record's Message (by Id order).
        /// Uses the stored Message::text format for hashing to avoid JSONB normalization mismatches during validation.
        /// </summary>
        public async Task<int> AddAsync(MessageModel record)
        {
            try
            {
                var command = JsonConvert.DeserializeObject<EventModel>(record.Message);

                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                {
                    await connect.OpenAsync();

                    var previousMessage = await connect.QueryFirstOrDefaultAsync<string>(
                        "SELECT Message::text FROM queue ORDER BY Id DESC LIMIT 1");
                    var previousMessageStr = previousMessage ?? string.Empty;

                    var insertSql = @"INSERT INTO queue(Message, Added, EventId, Username, EventStatusId, TopicId, tablename) 
                                     VALUES(Cast(@Message As json), now(), @EventId, @Username, @StatusId, @TopicId, NULLIF(@TableName, '')) 
                                     RETURNING Id, Message::text as StoredMessage;";

                    var inserted = await connect.QueryFirstAsync<(int Id, string StoredMessage)>(insertSql, new
                    {
                        record.Message,
                        record.EventId,
                        record.Username,
                        record.StatusId,
                        record.TopicId,
                        record.TableName
                    });

                    var storedMessage = inserted.StoredMessage ?? string.Empty;
                    var hash = _queueHashService.ComputeHash(previousMessageStr, storedMessage);

                    await connect.ExecuteAsync(
                        "UPDATE queue SET hash = @Hash WHERE Id = @Id",
                        new { Hash = hash, Id = inserted.Id });

                    return inserted.Id;
                }
            }
            catch (JsonReaderException ex)
            {
                var previewLength = Math.Min(record.Message?.Length ?? 0, 200);
                var preview = previewLength > 0 ? record.Message.Substring(0, previewLength) : string.Empty;
                _logger.LogError(
                    "MessageRepository: JSON deserialize failed for queue insert. Length={MessageLength}, preview={MessagePreview}, error={Error}",
                    record.Message?.Length ?? 0,
                    preview,
                    ex.Message);
                throw;
            }
            catch (NpgsqlException e)
            {
                _logger.LogError("Create message caused ngSQL exception : {0}", e.Message);
                throw new Exception(e.Message);
            }
        }

        public async Task UpdateStateAsync(int id, int newStatus, string error, int recordId, SpecimenPatientModel specimenModel, int stateId, string tableName = null)
        {
            try
            {
                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                {
                    var sql = @"Update Queue set EventStatusId = @Status, Error = @Error, RecordId = @RecordId, SpecimenId = @SpecimenId, PatientId = @PatientId, StateId = @StateId,
                                tablename = COALESCE(NULLIF(trim(@TableName), ''), tablename) Where Id = @Id";

                    await connect.ExecuteAsync(sql, new { Id = id, Status = newStatus, Error = error, @RecordId = recordId, @SpecimenId = specimenModel.SpecimenId, @PatientId = specimenModel.PatientId, @StateId = stateId, TableName = tableName } );
                }
            }
            catch (NpgsqlException e)
            {
                _logger.LogError("Error updating status to {1} : {0}", e.Message, newStatus);
                throw new Exception(e.Message);
            }
        }

        public async Task<MessageModel> GetSingleAsync(int id)
        {
            try
            {
                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                {
                    var sql = @"Select * from Queue Where Id = @Id;";

                    return await connect.QueryFirstAsync<MessageModel>(sql, new { Id = id });
                }
            }
            catch (NpgsqlException e)
            {
                _logger.LogError("Getting single message item caused ngSQL exception : {0}", e.Message);
                throw new Exception(e.Message);
            }
        }

        /// <inheritdoc />
        public async Task<Dictionary<int, string>> GetQueueChainAsync()
        {
            try
            {
                using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
                {
                    await connect.OpenAsync();
                    var rows = await connect.QueryAsync<(int, string)>(
                        "SELECT Id, Message::text FROM queue ORDER BY Id");
                    return rows?.ToDictionary(r => r.Item1, r => r.Item2 ?? string.Empty) ?? new Dictionary<int, string>();
                }
            }
            catch (NpgsqlException e)
            {
                _logger.LogError("GetQueueChainAsync caused ngSQL exception : {0}", e.Message);
                throw new Exception(e.Message);
            }
        }
    }
}

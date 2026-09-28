using arc.app.Common;
using arc.app.Config.Queries;
using arc.common.Models;
using arc.domain.Configuration.QueryConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.app.Monitoring
{
    /// <summary>
    /// Handles the QueueList query with hash chain validation.
    /// Fetches the full chain for validation, fetches filtered display records, validates each against the actual previous record in the chain (by Id), adds IsValid, and returns the list without Message.
    /// </summary>
    public class QueueListQueryHandler : IQueueListQueryHandler
    {
        private readonly IQueryAdapter _queryAdapter;
        private readonly IGenericRepository _genericRepository;
        private readonly IQueueHashService _queueHashService;
        private readonly IMessageRepository _messageRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueListQueryHandler"/> class.
        /// </summary>
        /// <param name="queryAdapter">Adapter for retrieving query configuration.</param>
        /// <param name="genericRepository">Generic repository for executing queries.</param>
        /// <param name="queueHashService">Service for validating hash chain.</param>
        /// <param name="messageRepository">Repository for retrieving the full queue chain for validation.</param>
        public QueueListQueryHandler(
            IQueryAdapter queryAdapter,
            IGenericRepository genericRepository,
            IQueueHashService queueHashService,
            IMessageRepository messageRepository)
        {
            _queryAdapter = queryAdapter;
            _genericRepository = genericRepository;
            _queueHashService = queueHashService;
            _messageRepository = messageRepository;
        }

        /// <inheritdoc />
        public async Task<string> GetQueueListAsync(QueryFilterConfig queryFilters, TokenInfoModel token)
        {
            var queryData = await _queryAdapter.GetQueryAsync("QueueList");
            if (queryData == null)
            {
                return "[]";
            }

            var chain = await _messageRepository.GetQueueChainAsync();

            var configWithMessageAndHash = CloneConfigWithMessageAndHash(queryData);
            configWithMessageAndHash.OrderBy = "Id";
            configWithMessageAndHash.Descending = false;

            _genericRepository.AddConfiguration("Queue");
            _genericRepository.AddToken(token);

            var rawJson = await _genericRepository.GetListAsync(configWithMessageAndHash, queryFilters);
            var records = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(rawJson);
            if (records == null || records.Count == 0)
            {
                return rawJson;
            }

            var sortedById = records.OrderBy(r => GetInt(r, "id")).ToList();

            foreach (var record in sortedById)
            {
                var currentId = GetInt(record, "id");
                var hash = GetString(record, "hash");

                var previousId = chain.Keys.Where(k => k < currentId).OrderByDescending(k => k).FirstOrDefault();
                var previousMessage = previousId == 0 ? string.Empty : (chain.GetValueOrDefault(previousId) ?? string.Empty);
                var currentMessage = chain.GetValueOrDefault(currentId, GetString(record, "message") ?? string.Empty);

                var isValid = _queueHashService.ValidateHash(previousMessage, currentMessage, hash);
                record["isValid"] = isValid;
                record.Remove("message");
            }

            var displayOrder = sortedById.OrderByDescending(r => GetDateTime(r, "added")).ToList();
            return JsonConvert.SerializeObject(displayOrder);
        }

        private static QueryConfig CloneConfigWithMessageAndHash(QueryConfig source)
        {
            var json = JsonConvert.SerializeObject(source);
            var config = JsonConvert.DeserializeObject<QueryConfig>(json);
            config.Fields ??= new List<QueryField>();
            if (!config.Fields.Any(f => f.Name.Equals("Message", StringComparison.OrdinalIgnoreCase)))
            {
                config.Fields.Add(new QueryField { Name = "Message", Type = "string" });
            }
            if (!config.Fields.Any(f => f.Name.Equals("Hash", StringComparison.OrdinalIgnoreCase)))
            {
                config.Fields.Add(new QueryField { Name = "Hash", Type = "string" });
            }
            return config;
        }

        private static int GetInt(Dictionary<string, object> record, string key)
        {
            var keys = record.Keys.FirstOrDefault(k => k.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (keys == null) return 0;
            var val = record[keys];
            if (val == null) return 0;
            if (val is int i) return i;
            if (val is long l) return (int)l;
            return int.TryParse(val.ToString(), out var parsed) ? parsed : 0;
        }

        private static string GetString(Dictionary<string, object> record, string key)
        {
            var keys = record.Keys.FirstOrDefault(k => k.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (keys == null) return null;
            var val = record[keys];
            return val?.ToString();
        }

        private static DateTime GetDateTime(Dictionary<string, object> record, string key)
        {
            var keys = record.Keys.FirstOrDefault(k => k.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (keys == null) return default;
            var val = record[keys];
            if (val == null) return default;
            if (val is DateTime dt) return dt;
            if (val is DateTimeOffset dto) return dto.DateTime;
            return DateTime.TryParse(val.ToString(), out var parsed) ? parsed : default;
        }
    }
}

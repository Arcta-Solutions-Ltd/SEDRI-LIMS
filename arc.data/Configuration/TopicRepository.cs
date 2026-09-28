using arc.app.Configuration;
using arc.common.Models.Config;
using arc.domain.Configuration.ListsConfig;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// Loads topic display strings from <c>topictranslation</c> for role permissions and other UI that groups events by topic.
    /// </summary>
    public class TopicRepository : ITopicRepository
    {
        private readonly IOptionsMonitor<DataOptions> _options;
        private readonly ILogger _logger;
        private readonly ISqlQuery _sqlQuery;
        public TopicRepository(IOptionsMonitor<DataOptions> options, ILogger logger, ISqlQuery sqlQuery)
        {
            _options = options;
            _logger = logger;
            _sqlQuery = sqlQuery;
        }

        /// <summary>
        /// Returns all rows from <c>topictranslation</c> (<c>listitemname</c> matches <c>Event.Topic</c>; <c>displayedtopic</c> is localized display text).
        /// </summary>
        public async Task<List<TopicTranslationModel>> GetTopicsForTranslationAsync()
        {
            using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
            {
                var sql = @"select listitemname, displayedtopic from topictranslation";

                var result = await connect.QueryAsync<TopicTranslationModel>(sql);

                var list = result.ToList();
                _logger.LogDebug("Loaded {Count} topic translation rows", list.Count);
                if (list.Count == 0)
                {
                    _logger.LogWarning("topictranslation returned zero rows; event permission topic labels will be missing");
                }

                return list;
            }
        }

    }
}

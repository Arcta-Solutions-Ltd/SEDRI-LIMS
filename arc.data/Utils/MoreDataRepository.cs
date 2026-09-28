using arc.app.Common;
using arc.common.Utils;
using arc.data.Configuration;
using Dapper;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Utils
{
    /// <summary>
    /// Repository for reading and merging JSON more-data payloads stored per-entity.
    /// </summary>
    public class MoreDataRepository : IMoreDataRepository
    {
        private readonly IJsonWholeStructureFieldsCollector _jsonConverter;
        private readonly IOptionsMonitor<DataOptions> _options;
        private readonly ILogWriter _logWriter;

        /// <summary>
        /// Creates a new instance of <see cref="MoreDataRepository"/>.
        /// </summary>
        /// <param name="jsonConverter">Extracts structured fields from JSON payloads.</param>
        /// <param name="options">Database connection configuration.</param>
        /// <param name="logWriter">Writes structured log entries during processing.</param>
        public MoreDataRepository(IJsonWholeStructureFieldsCollector jsonConverter, IOptionsMonitor<DataOptions> options, ILogWriter logWriter)
        {
            _jsonConverter = jsonConverter;
            _options = options;
            _logWriter = logWriter;
        }

        /// <inheritdoc />
        public async Task<string> CombineWithExistingFieldAsync(string newData, string tableName, string id, NpgsqlConnection connect = null)
        {
            var jsonObject = new JObject();
            var ownsConnection = connect == null;

            if (ownsConnection)
            {
                connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection);
                await connect.OpenAsync();
            }

            _logWriter.LogInfo(
                $"CombineWithExistingFieldAsync: table={tableName}, id={id}, connection={(ownsConnection ? "new" : "shared")}",
                nameof(MoreDataRepository),
                nameof(CombineWithExistingFieldAsync));

            try
            {
                var sql = "Select MoreData from " + tableName + " where Id = @Id";
                var result = await connect.QueryFirstAsync<string>(sql, new { Id = int.Parse(id) });

                result = result == null ? "{}" : result;

                var newFields = _jsonConverter.GetStructure(newData);
                var oldFields = _jsonConverter.GetStructure(result);

                foreach (var field in newFields)
                {
                    var match = oldFields.Where(f => f.Label.ToLower() == field.Label.ToLower());
                    if (match.Count() > 0)
                    {
                        var existing = match.First();
                        existing.Contents = field.Contents;
                        existing.Label = field.Label;
                    }
                    else
                    {
                        oldFields.Add(field);
                    }
                }

                foreach (var field in oldFields)
                {
                    if (!string.IsNullOrEmpty(field.Contents))
                    {
                        jsonObject.Add(field.Label, field.Contents);
                    }
                }
            }
            finally
            {
                if (ownsConnection)
                {
                    await connect.DisposeAsync();
                }
            }

            return jsonObject.ToString();
        }
    }
}

using arc.app.Configuration;
using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    /// <summary>
    /// Repository that provides simple state value lookups for arbitrary tables/fields.
    /// </summary>
    public class StateRepository : IStateRepository
    {
        private readonly IOptionsMonitor<DataOptions> _options;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateRepository"/> class.
        /// </summary>
        /// <param name="options">Configuration monitor for database connection options.</param>
        /// <param name="logger">Logger used for diagnostics.</param>
        public StateRepository(IOptionsMonitor<DataOptions> options, ILogger logger)
        {
            _options = options;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a single field value from the specified table for the given primary key.
        /// </summary>
        /// <param name="id">Primary key of the row to read.</param>
        /// <param name="table">Target table name.</param>
        /// <param name="field">Target field/column name.</param>
        /// <returns>The string value of the requested field.</returns>
        public async Task<string> GetStateAsync(string id, string table, string field)
        {
            using (var connect = new NpgsqlConnection(_options.CurrentValue.ArcConnection))
            {
                var sql = "select " + field + " from " + table + " where id = @Id";

                var result = await connect.QueryFirstAsync<string>(sql, new {Id = int.Parse(id)});

                return result;
            }
        }
    }
}

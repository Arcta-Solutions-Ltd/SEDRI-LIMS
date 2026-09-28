using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Configuration
{
    internal class GetListValueFromIdQuery : IQueryReturningString
    {
        public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = int.Parse(queryFilters.Parameters[0].Value);

            var sql = "select Value From ListItem Where Id = @Id";

            var result = await connect.QueryFirstAsync<string>(sql, new { Id = id });

            return result;
        }
    }
}

using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    internal class IsIqcTestProfileOrganismUsedByDefaultQuery : IQueryReturningType<bool>
    {
        public async Task<bool> ExecuteAsync(NpgsqlConnection connection, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("id");

            var sql = @"select usebydefault from iqctestprofileqcorganisms where id = @id";

            return await connection.QuerySingleAsync<bool>(sql, new { id });
        }
    }
}

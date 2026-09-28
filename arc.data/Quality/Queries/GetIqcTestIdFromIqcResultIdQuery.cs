using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Quality.Queries
{
    internal class GetIqcTestIdFromIqcResultIdQuery : IQueryReturningType<int>
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connection, QueryFilterConfig queryFilters)
        {
            var sql = @"select ir.iqctestid as id from iqcresults ir where ir.id = @iqcResultId";

            return await connection.QueryFirstAsync<int>(sql, new { iqcResultId = queryFilters.GetIntegerValue("iqcResultId") });
        }
    }
}

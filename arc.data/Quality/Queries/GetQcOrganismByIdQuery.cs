using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Quality;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Quality
{
    internal class GetQcOrganismByIdQuery : IQueryReturningType<QcOrganism>
    {
        public async Task<QcOrganism> ExecuteAsync(NpgsqlConnection connection, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("id");

            var sql = @"select * from qcorganisms where id = @Id";

            return await connection.QueryFirstAsync<QcOrganism>(sql, new { Id = id });
        }
    }
}

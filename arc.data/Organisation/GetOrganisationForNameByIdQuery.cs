using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Organisation
{
    internal class GetOrganisationForNameByIdQuery : IQueryReturningType<OptionsConfig>
    {
        public async Task<OptionsConfig> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

            var sql = @"select id as key, organisationname As text, parentorganisationid as ParentKey from organisation where id = @Id";
            return await connect.QueryFirstAsync<OptionsConfig>(sql, new { Id = int.Parse(id.Value) });
        }
    }
}

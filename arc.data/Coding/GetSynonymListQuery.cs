using arc.data.model.Organism;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class GetSynonymListQuery : IQueryReturningType<List<OrganismSynonymDataModel>>
    {
        public async Task<List<OrganismSynonymDataModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

            var sql = @"select * from organismsynonyms where organismId = @organismid";

            var result = await connect.QueryAsync<OrganismSynonymDataModel>(sql, new { organismId = int.Parse(id.Value) });

            return result.ToList();
        }
    }
}

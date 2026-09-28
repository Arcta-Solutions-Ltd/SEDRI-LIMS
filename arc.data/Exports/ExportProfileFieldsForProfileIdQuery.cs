using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    public class ExportProfileFieldsForProfileIdQuery : IQueryReturningType<List<ExportProfileFieldModel>>
    {
        public async Task<List<ExportProfileFieldModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "exportprofileid").FirstOrDefault();
            if (id == null)
            {
                id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();
            }

            var sql = @"select ep.* from exportprofilerecord ep where exportprofileid = @Id order by ordernumber";

            var exportProfile = await connect.QueryAsync<ExportProfileFieldModel>(sql, new { Id = int.Parse(id.Value) });

            return exportProfile.ToList();
        }
    }
}

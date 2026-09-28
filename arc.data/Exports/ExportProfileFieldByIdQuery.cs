using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    public class ExportProfileFieldByIdQuery : IQueryReturningType<ExportProfileFieldModel>
    {
        public async Task<ExportProfileFieldModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "exportprofileid").First();

            var sql = @"select ep.* from exportprofilerecord ep where id = @Id";

            var exportProfile = await connect.QueryFirstAsync<ExportProfileFieldModel>(sql, new { Id = int.Parse(id.Value) });

            return exportProfile;
        }
    }
}

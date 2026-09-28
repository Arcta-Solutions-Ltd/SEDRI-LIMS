using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Import
{
    internal class EditImportProfileQuery : IQueryReturningType<ExportProfileModel>
    {
        public async Task<ExportProfileModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

            var sql = @"select id, name, description, includeheaderrow, tablenameid from importprofile where Id = @Id";

            var exportProfile = await connect.QueryFirstAsync<ExportProfileModel>(sql, new { Id = int.Parse(id.Value) });

            return exportProfile;
        }
    }
}

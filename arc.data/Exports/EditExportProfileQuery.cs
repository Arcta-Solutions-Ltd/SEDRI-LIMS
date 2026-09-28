using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    internal class EditExportProfileQuery : IQueryReturningType<ExportProfileModel>
    {
        public async Task<ExportProfileModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

            var sql = @"select id, name, description,  modifieddate AT TIME ZONE 'UTC' As modifieddate from exportprofile where Id = @Id";

            var exportProfile = await connect.QueryFirstAsync<ExportProfileModel>(sql, new { Id = int.Parse(id.Value) });

            return exportProfile;
        }
    }
}

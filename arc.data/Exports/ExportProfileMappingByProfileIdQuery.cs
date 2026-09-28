using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    /// <summary>
    /// Query that returns the mapping row for a given export profile id, or null if none exists yet.
    /// </summary>
    internal class ExportProfileMappingByProfileIdQuery : IQueryReturningType<ExportProfileMappingModel?>
    {
        /// <inheritdoc />
        public async Task<ExportProfileMappingModel?> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var idParam = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("ExportProfileId", System.StringComparison.OrdinalIgnoreCase));
            if (idParam == null || string.IsNullOrEmpty(idParam.Value))
            {
                return null;
            }

            if (!int.TryParse(idParam.Value, out var profileId))
            {
                return null;
            }

            var sql = @"SELECT id, exportprofileid AS ExportProfileId, format, structure::text AS Structure, modifieddate
                FROM exportprofilemapping
                WHERE exportprofileid = @ProfileId";

            return await connect.QueryFirstOrDefaultAsync<ExportProfileMappingModel>(sql, new { ProfileId = profileId });
        }
    }
}

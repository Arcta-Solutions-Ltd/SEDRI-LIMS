using arc.common.Models.Export;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports
{
    /// <summary>
    /// Query that returns a single export schedule by ID for editing.
    /// </summary>
    internal class ExportScheduleByIdQuery : IQueryReturningType<ExportScheduleModel?>
    {
        /// <inheritdoc />
        public async Task<ExportScheduleModel?> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var idParam = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("id", System.StringComparison.OrdinalIgnoreCase));
            if (idParam == null || string.IsNullOrEmpty(idParam.Value))
            {
                return null;
            }

            var id = int.Parse(idParam.Value);
            var sql = @"SELECT id, exportprofileid, name, filter, frequency, timeofday, dayofmonth, incrementalonly, enabled, modifieddate, outputdirectory, changestoinclude
                FROM exportschedule WHERE id = @Id";

            var result = await connect.QueryFirstOrDefaultAsync<ExportScheduleModel>(sql, new { Id = id });
            return result;
        }
    }
}

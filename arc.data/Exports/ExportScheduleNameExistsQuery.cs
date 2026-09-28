using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports;

/// <summary>
/// Query that checks if an export schedule with the given name exists for the profile.
/// </summary>
internal class ExportScheduleNameExistsQuery : IQueryReturningType<bool>
{
    /// <inheritdoc />
    public async Task<bool> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var exportProfileIdParam = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("ExportProfileId", System.StringComparison.OrdinalIgnoreCase));
        var nameParam = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("Name", System.StringComparison.OrdinalIgnoreCase));
        var excludeParam = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("ExcludeScheduleId", System.StringComparison.OrdinalIgnoreCase));

        if (exportProfileIdParam == null || !int.TryParse(exportProfileIdParam.Value, out var exportProfileId) || string.IsNullOrWhiteSpace(nameParam?.Value))
        {
            return false;
        }

        var name = nameParam.Value.Trim();
        int? excludeId = null;
        if (excludeParam != null && int.TryParse(excludeParam.Value, out var ex) && ex > 0)
        {
            excludeId = ex;
        }

        var sql = excludeId.HasValue
            ? @"SELECT EXISTS(
                SELECT 1 FROM exportschedule
                WHERE exportprofileid = @ExportProfileId
                AND LOWER(TRIM(name)) = LOWER(TRIM(@Name))
                AND id != @ExcludeId
            )"
            : @"SELECT EXISTS(
                SELECT 1 FROM exportschedule
                WHERE exportprofileid = @ExportProfileId
                AND LOWER(TRIM(name)) = LOWER(TRIM(@Name))
            )";

        return excludeId.HasValue
            ? await connect.ExecuteScalarAsync<bool>(sql, new { ExportProfileId = exportProfileId, Name = name, ExcludeId = excludeId.Value })
            : await connect.ExecuteScalarAsync<bool>(sql, new { ExportProfileId = exportProfileId, Name = name });
    }
}

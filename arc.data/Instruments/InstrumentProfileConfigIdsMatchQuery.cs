using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Returns whether a comma-separated list of config ids (and/or confignames) matches a given <c>configs.configname</c>.
/// </summary>
internal class InstrumentProfileConfigIdsMatchQuery : IQueryReturningType<bool>
{
    public async Task<bool> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var idList = queryFilters.GetStringValue("profileids");
        var testName = queryFilters.GetStringValue("testname");
        if (string.IsNullOrWhiteSpace(idList) || string.IsNullOrWhiteSpace(testName))
            return false;

        var sql = @"SELECT EXISTS (
            SELECT 1 FROM configs c
            WHERE lower(trim(c.configname)) = lower(trim(@testName))
            AND EXISTS (
                SELECT 1 FROM unnest(string_to_array(@profileIds, ',')) AS x(idtext)
                WHERE trim(x.idtext) <> ''
                AND (c.id::text = trim(x.idtext) OR lower(trim(c.configname)) = lower(trim(x.idtext)))
            ))";
        return await connect.QueryFirstAsync<bool>(sql, new { profileIds = idList, testName });
    }
}

using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Returns <c>culture.culturenumber</c> as text for a culture id.
/// </summary>
internal class CultureNumberTextByCultureIdQuery : IQueryReturningString
{
    public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        if (!queryFilters.TryParseIntegerValue("cultureid", out var cultureId, 0) || cultureId <= 0)
            return null;

        const string sql = "select trim(culturenumber::text) from culture where id = @cultureId";
        return await connect.QueryFirstOrDefaultAsync<string>(sql, new { cultureId });
    }
}

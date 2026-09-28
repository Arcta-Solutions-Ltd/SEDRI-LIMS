using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Returns <c>Culture.SpecimenId</c> for a culture id.
/// </summary>
internal class SpecimenIdByCultureIdQuery : IQueryReturningType<int>
{
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var sql = @"select specimenid from culture where id = @cultureid";
        return await connect.QueryFirstAsync<int>(sql, new { cultureid = queryFilters.GetIntegerValue("cultureid") });
    }
}

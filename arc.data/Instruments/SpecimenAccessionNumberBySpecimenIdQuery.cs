using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Returns <c>specimen.accessionnumber</c> for a specimen id.
/// </summary>
internal class SpecimenAccessionNumberBySpecimenIdQuery : IQueryReturningString
{
    public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        if (!queryFilters.TryParseIntegerValue("specimenid", out var specimenId, 0) || specimenId <= 0)
            return null;

        const string sql = "select accessionnumber::text from specimen where id = @specimenId";
        return await connect.QueryFirstOrDefaultAsync<string>(sql, new { specimenId });
    }
}

using arc.data.model.Instruments;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Instruments;

/// <summary>
/// Loads one <c>instrumentresults</c> row by primary key.
/// </summary>
internal class InstrumentResultByIdQuery : IQueryReturningType<InstrumentResultsDataModel>
{
    async Task<InstrumentResultsDataModel> IQueryReturningType<InstrumentResultsDataModel>.ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        if (!queryFilters.TryParseIntegerValue("id", out var id, 0) || id <= 0)
            return null;

        const string sql = "select * from instrumentresults where id = @id";
        return await connect.QueryFirstOrDefaultAsync<InstrumentResultsDataModel>(sql, new { id });
    }
}

using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding;

internal class AntibioticEntryByCodeQuery : IQueryReturningType<AntibioticListModel>
{
    public async Task<AntibioticListModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var code = queryFilters.GetStringValue("code");
        var codingId = queryFilters.GetIntegerValue("codingid");

        var sql = @"select a.id, a.antibioticname, ac.code from antibiotic a
                        inner join antibioticcoding ac on ac.antibioticid = a.id and ac.codingid = @codingid
                        where ac.code = @code and CodingId = @codingId";

        return await connect.QueryFirstOrDefaultAsync<AntibioticListModel>(sql, new { code, codingId });
    }
}

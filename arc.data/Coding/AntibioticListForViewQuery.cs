using arc.common.Models.Coding;
using arc.data.Extensions;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

internal class AntibioticListForViewQuery : IQueryReturningType<List<AntibioticListModel>>
{
    public async Task<List<AntibioticListModel>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var codingId = queryFilters.GetIntegerValue("codingid");
        var wildcardAntibioticName = queryFilters.GetStringValue("antibioticname").ToLower().ToSqlWildcard();
        var wildcardCode = queryFilters.GetStringValue("code").ToLower().ToSqlWildcard();

        var orderField = queryFilters.OrderBy.ToLower().Trim();
        var codeField = codingId == 21 ? "a.code" : "ac.code";
        var idField = codingId == 21 ? "a.id" : "ac.id";
        var orderBy = orderField == "code" ? codeField : "a.antibioticname";
        var desc = queryFilters.OrderDescending ? "desc" : "";

        var sql = $"""
            select {idField}, a.antibioticname, {codeField} from antibiotic a
            inner join antibioticcoding ac on ac.antibioticid = a.id and ac.codingid = @codingid
            where LOWER(a.antibioticname) like @wildcardAntibioticName or LOWER({codeField}) like @wildcardCode
            order by {orderBy} {desc}
            """;

        var result = await connect.QueryAsync<AntibioticListModel>(sql, new { codingId, wildcardAntibioticName, wildcardCode });

        return result.ToList();
    }
}

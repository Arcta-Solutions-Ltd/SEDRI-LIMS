using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class AntibioticEntryByIdQuery : IQueryReturningType<AntibioticListModel>
    {
        public async Task<AntibioticListModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.GetIntegerValue("id");
            var codingId = queryFilters.GetIntegerValue("metafcodingid");
            var codeField = codingId == 21 ? "a.code" : "ac.code";
            var idField = codingId == 21 ? "a.id" : "ac.id";

            var sql = @"select " + idField + @", a.antibioticname, " + codeField + @" from antibiotic a
                        inner join antibioticcoding ac on ac.antibioticid = a.id and ac.codingid = @codingid
                        where " + idField + @" = @Id";

            var result = await connect.QueryFirstAsync<AntibioticListModel>(sql, new { Id = id });

            return result;
        }
    }
}

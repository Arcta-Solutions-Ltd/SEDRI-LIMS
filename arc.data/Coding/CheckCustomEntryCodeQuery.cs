using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class CheckCustomEntryCodeQuery : IQueryReturningInteger
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var code = queryFilters.Parameters.Where(p => p.Key.ToLower() == "code").First();
            var codingId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "metafcodingid").First();

            var sql = @"select count(a.Id) from Additional a
                        inner join organism o on o.additionalId = a.Id
                        inner join organismcoding oc on oc.organismId = o.Id
                        where oc.Code = @Code and oc.CodingId = @CodingId";
            var result = await connect.QueryFirstAsync<int>(sql, new { Code = code.Value, CodingId = int.Parse(codingId.Value) });
            return result;
        }
    }
}


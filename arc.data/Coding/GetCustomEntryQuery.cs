using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding
{
    internal class GetCustomEntryQuery : IQueryReturningInteger
    {
        public async Task<int> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var name = queryFilters.Parameters.Where(p => p.Key.ToLower() == "description").First();
            var codingId = queryFilters.Parameters.Where(p => p.Key.ToLower() == "metafcodingid").First();

            var sql = @"select count(a.Id) from Additional a
                        inner join organism o on o.additionalId = a.Id
                        inner join organismcoding oc on oc.organismId = o.Id
                        where a.Name = @Name and oc.CodingId = @CodingId";
            var result = await connect.QueryFirstAsync<int>(sql, new { Name = name.Value, CodingId = int.Parse(codingId.Value) });
            return result;
        }
    }
}

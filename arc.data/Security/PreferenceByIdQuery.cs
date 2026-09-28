using arc.domain;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Security
{
    internal class PreferenceByIdQuery : IQueryReturningType<Preference>
    {
        public async Task<Preference> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.Where(p => p.Key.ToLower() == "id").First();

            var sql = @"select moredata::jsonb->>'DataFullScreen' as DataFullScreen, moredata::jsonb->>'ASTRowRemovalConfirmation' as ASTRowRemovalConfirmation from users where id = @Id";

            return await connect.QueryFirstAsync<Preference>(sql, new { Id = int.Parse(id.Value) });
        }
    }
}

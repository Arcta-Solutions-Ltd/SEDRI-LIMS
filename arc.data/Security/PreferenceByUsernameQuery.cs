using arc.common.Models.User;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Security
{
    internal class PreferenceByUsernameQuery : IQueryReturningType<PreferenceConfigModel>
    {
        public async Task<PreferenceConfigModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var username = queryFilters.Parameters.Where(p => p.Key.ToLower() == "username").First();

            var sql = @"select moredata::jsonb->>'DataFullScreen' as DataFullScreen, moredata::jsonb->>'ASTRowRemovalConfirmation' as ASTRowRemovalConfirmation, coalesce((moredata::jsonb->'FilterPresets')::text, '{}') as FilterPresets, coalesce((moredata::jsonb->'ColumnLayouts')::text, '{}') as ColumnLayouts, (moredata::jsonb->'HomeDashboard')::text as HomeDashboard from users where username = @Username";

            var result = await connect.QueryFirstAsync<PreferenceConfigModel>(sql, new { UserName = username.Value });

            if (string.IsNullOrWhiteSpace(result.DataFullScreen))
            {
                result.DataFullScreen = "No";
            }

            if (string.IsNullOrWhiteSpace(result.ASTRowRemovalConfirmation))
            {
                result.ASTRowRemovalConfirmation = "No";
            }

            return result;
        }
    }
}

using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Security
{
    /// <summary>
    /// Retrieves the MoreData JSON for a user by Id.
    /// </summary>
    internal class GetUserMoreDataByIdQuery : IQueryReturningString
    {
        /// <summary>
        /// Executes the query to return the MoreData JSON string for the specified user.
        /// </summary>
        /// <param name="connect">An open database connection.</param>
        /// <param name="queryFilters">Filter bag containing the user Id.</param>
        /// <returns>The MoreData JSON string, or null if not set.</returns>
        public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
        {
            var id = queryFilters.Parameters.First(p => p.Key.ToLower() == "id").Value;

            var sql = @"select moredata::text from Users where Id = @Id";
            var result = await connect.QueryFirstOrDefaultAsync<string>(sql, new { Id = int.Parse(id) });
            return result;
        }
    }
}

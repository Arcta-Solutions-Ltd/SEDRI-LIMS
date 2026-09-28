using arc.common.ExtensionMethods;
using arc.common.Models;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Security.Token;

/// <summary>
/// Represents a query for retrieving token information based on an email address.
/// </summary>
internal class TokenByEmailQuery
{
    /// <summary>
    /// Executes the query to retrieve token information for a specified email address.
    /// </summary>
    /// <param name="connect">The database connection used to execute the query.</param>
    /// <param name="queryFilters">Contains filtering parameters for the query.</param>
    /// <returns>A Task resolving to a TokenInfoModel with token details.</returns>
    public async Task<TokenInfoModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var email = queryFilters.Parameters.Where(p => p.Key.IsSameAs("email")).First().Value;
        var labId = queryFilters.Parameters.Where(p => p.Key.IsSameAs("labid")).First().Value;

        var sql = @"SELECT u.id, u.username FROM Users u WHERE Email = @Email";

        var result = await connect.QueryAsync<TokenInfoModel>(sql, new { Email = email });
        var userRecord = result.First();

        return userRecord;
    }
}

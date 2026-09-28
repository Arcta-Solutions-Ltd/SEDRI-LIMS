using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Security.User;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Security;

/// <summary>
/// Represents a query to retrieve a user by ID, including associated laboratory and role information.
/// </summary>
internal class UserByIdQuery : IQueryReturningString
{
    /// <summary>
    /// Executes the query asynchronously and returns user details as a JSON string.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="queryFilters">The query filter configuration.</param>
    /// <returns>A JSON-formatted string containing the user's information.</returns>
    public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var sql = @"select u.id, u.enabled, u.firstname, u.lastname, u.username, u.email, u.moredata, u.lastmodifieddate, 
                            lu.LaboratoryId,
                            lo.OrganisationId,
                            ur.Roles 
                    from users u
                    left outer join (
                        SELECT min(UserId) as UserId, 
                               string_agg(laboratoryid::text, ',') As LaboratoryId 
                        FROM LaboratoryUser 
                        WHERE UserId = @UserId 
                    ) As lu on lu.UserId = u.Id
                    left outer join (
                        SELECT min(UserId) as UserId, 
                               string_agg(organisationid::text, ',') As OrganisationId 
                        FROM OrganisationUser 
                        WHERE UserId = @UserId 
                    ) As lo on lo.UserId = u.Id
                    inner join (
                        SELECT min(UserId) as UserId, 
                               string_agg(roleid::text, ',') As Roles 
                        FROM UserRole 
                        WHERE UserId = @UserId 
                    ) As ur on ur.UserId = u.Id";

        var result = await connect.QueryAsync<User>(sql, new { UserId = int.Parse(queryFilters.Parameters[0].Value) });

        return JsonConvert.SerializeObject(result.First());
    }
}

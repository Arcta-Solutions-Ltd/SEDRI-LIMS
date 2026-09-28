using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Security.User;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Security;

/// <summary>
/// A query that retrieves the details of a single user for display in a user list.
/// This query returns user information along with aggregated laboratory names, organisation names, 
/// and role names associated with the user.
/// </summary>
internal class SingleUserForUserListQuery : IQueryReturningString
{
    /// <summary>
    /// Executes the query asynchronously to retrieve a single user's information.
    /// The query selects basic user details from the "users" table, and uses subqueries to aggregate 
    /// related laboratory, organisation, and role names into comma-separated strings.
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> used to execute the SQL query.
    /// </param>
    /// <param name="queryFilters">
    /// A <see cref="QueryFilterConfig"/> that contains query parameters. The first parameter is expected to be the user ID.
    /// </param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a JSON string representing the user's details.
    /// </returns>
    public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var sql = @"SELECT s.Id, s.Username, s.FirstName, s.LastName, s.Enabled, 
            (SELECT STRING_AGG(l.laboratoryname, ', ')
             FROM laboratoryuser lu
             JOIN laboratory l ON l.id = lu.laboratoryid
             WHERE lu.userid = s.id) AS laboratories,
            (SELECT STRING_AGG(o.organisationname, ', ')
             FROM organisationuser ou
             JOIN organisation o ON o.id = ou.organisationid
             WHERE ou.userid = s.id) AS organisations,
            (
              SELECT STRING_AGG(r.rolename, ', ')
              FROM userrole ur
              JOIN role r ON r.id = ur.roleid
              WHERE ur.userid = s.id
            ) AS roles
            FROM users s 
            where s.Id = @UserId";

        var result = await connect.QueryAsync<User>(sql, new { UserId = int.Parse(queryFilters.Parameters[0].Value) });

        return JsonConvert.SerializeObject(result.First());
    }
}

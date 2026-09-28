using arc.data.Extensions;
using arc.data.Utils;
using arc.domain.Configuration.QueryFiltersConfig;
using arc.domain.Security.User;
using Dapper;
using Newtonsoft.Json;
using Npgsql;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace arc.data.Security;

/// <summary>
/// Query that returns a JSON string containing a list of users along with aggregated laboratory and organisation names.
/// The laboratory and organisation names for each user are concatenated into comma-separated strings.
/// </summary>
internal class UserListQuery : IQueryReturningString
{
    /// <summary>
    /// Executes the user list query asynchronously. The query selects users and adds two extra columns:
    /// one for the concatenated laboratory names and another for the concatenated organisation names.
    /// It also applies filtering based on provided query filters.
    /// </summary>
    /// <param name="connect">An open <see cref="NpgsqlConnection"/> to execute the query.</param>
    /// <param name="queryFilters">The configuration of query filters which may include username, firstname, lastname, etc.</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a JSON string representing
    /// the list of users with their laboratory and organisation names.
    /// </returns>
    public async Task<string> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var roleJoin = "";
        if (queryFilters.TryGetStringValue("roleid", out var roleId))
        {
            roleJoin = $"inner join userrole ur on ur.userid = s.id inner join role ro on ro.id = ur.roleid and ro.id in ({roleId}) ";
        }

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
            FROM users s "
            + roleJoin + GetWhereClause(queryFilters) +
            " order by " + ListQueryOrderByUtil.GetOrderByClause(queryFilters, "s.Username");

        var result = await connect.QueryAsync<User>(sql);
        return JsonConvert.SerializeObject(result);
    }

    /// <summary>
    /// Constructs a WHERE clause based on the provided query filter values.
    /// It checks for filters on username, lastname, and firstname and constructs corresponding 
    /// filtering conditions using the ILIKE operator for case-insensitive matching.
    /// </summary>
    /// <param name="queryFilters">The filter configuration containing potential filter criteria.</param>
    /// <returns>
    /// A WHERE clause string starting with "where" if any filters are provided; otherwise, an empty string.
    /// </returns>
    private static string GetWhereClause(QueryFilterConfig queryFilters)
    {
        List<string> clauses = new List<string>();

        if (queryFilters.TryGetStringValue("username", out var username))
        {
            clauses.Add($"s.username ilike '{username.ToSqlStartsWith()}'");
        }
        if (queryFilters.TryGetStringValue("lastname", out var lastname))
        {
            clauses.Add($"s.lastname ilike '{lastname.ToSqlStartsWith()}'");
        }
        if (queryFilters.TryGetStringValue("firstname", out var firstname))
        {
            clauses.Add($"s.firstname ilike '{firstname.ToSqlStartsWith()}'");
        }

        return clauses.Count == 0 ? "" :
            $"where {string.Join(" or ", clauses)}";
    }
}

//using arc.common.Models.Security;
//using arc.domain.Configuration.QueryFiltersConfig;
//using Dapper;
//using Npgsql;
//using System.Linq;
//using System.Threading.Tasks;

//namespace arc.data.Security;

/// <summary>
/// Query for retrieving a user's details along with their associated laboratories and organisations.
/// </summary>
//internal class GetLaboratoriesAndOrganisationsForUserQuery : IQueryReturningType<LaboratoryOrganisationModel>
//{
    /// <summary>
    /// Executes an asynchronous query that fetches user information, including aggregated laboratory and organisation IDs,
    /// and returns the result as a JSON string.
    /// </summary>
    /// <param name="connect">The NpgsqlConnection instance used to communicate with the database.</param>
    /// <param name="queryFilters">A configuration object containing query parameters. The first parameter's value is expected to be the user's ID.</param>
    /// <returns>A JSON string representing the user's data along with their laboratories and organisations.</returns>
//    public async Task<LaboratoryOrganisationModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
//    {
//        var username = queryFilters.GetStringValue("username");

//        var sql = @"SELECT s.Id, s.Username, s.FirstName, s.LastName, s.Enabled, 
//            (SELECT STRING_AGG(cast(l.id as varchar(10)), ', ')
//             FROM laboratoryuser lu
//             JOIN laboratory l ON l.id = lu.laboratoryid
//             WHERE lu.userid = s.id) AS laboratories,
//            (SELECT STRING_AGG(cast(o.id as varchar(10)), ', ')
//             FROM organisationuser ou
//             JOIN organisation o ON o.id = ou.organisationid
//             WHERE ou.userid = s.id) AS organisations
//            FROM users s 
//            where s.username = @Username";

//        var result = await connect.QueryAsync<LaboratoryOrganisationModel>(sql, new { username });
//        return result.First();
//    }
//}


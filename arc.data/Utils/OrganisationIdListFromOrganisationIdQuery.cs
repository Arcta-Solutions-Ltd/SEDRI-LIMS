using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Utils;

/// <summary>
/// Recursively retrieves an organisation’s ID and all of its descendant organisation IDs.
/// </summary>
internal class OrganisationIdListFromOrganisationIdQuery
{
    /// <summary>
    /// Executes a recursive query to build a list containing the specified organisationId
    /// and all child organisation IDs found in the <c>organisation</c> table.
    /// </summary>
    /// <param name="connect">
    /// An open <see cref="NpgsqlConnection"/> against which the recursive query will run.
    /// </param>
    /// <param name="organisationId">
    /// The root organisation ID from which to begin collecting descendant IDs.
    /// </param>
    /// <returns>
    /// A <see cref="Task{List}"/> containing the original <paramref name="organisationId"/>
    /// plus any IDs of child organisations (and their children) discovered during recursion.
    /// </returns>
    public async Task<List<int>> ExecuteAsync(NpgsqlConnection connect, int organisationId)
    {
        var returnIds = new List<int>() { organisationId };

        var sql = @"select * from organisation where parentorganisationid = @organisationId";

        var results = await connect.QueryAsync<int>(sql, new { organisationId });

        if (results.Any())
        {
            foreach (var result in results)
            {
                var newIds = await new OrganisationIdListFromOrganisationIdQuery().ExecuteAsync(connect, result);
                returnIds.AddRange(newIds);
            }
        }

        return returnIds.Distinct().ToList();
    }
}


using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Returns expert rule id and name pairs for the ids supplied in the comma-separated <c>ids</c> query filter.
/// Kept deliberately light so display code can resolve a stored rule id without loading the rule's
/// conditions and actions the way the edit form does.
/// </summary>
internal class ExpertRuleNamesByIdsQuery : IQueryReturningType<List<OptionsConfig>>
{
    /// <summary>
    /// Executes the query.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Must contain <c>ids</c> holding comma-separated expert rule ids.</param>
    /// <returns>Options with Key = expert rule id and Text = expert rule name.</returns>
    public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        if (!queryFilters.TryGetStringValue("ids", out var idsValue) || string.IsNullOrWhiteSpace(idsValue))
        {
            return [];
        }

        var ids = idsValue
            .Split(',')
            .Select(part => int.TryParse(part.Trim(), out var id) ? id : 0)
            .Where(id => id > 0)
            .Distinct()
            .ToArray();

        if (ids.Length == 0)
        {
            return [];
        }

        const string sql = @"select id::text as key, expertrulename as text from expertrule where id = ANY(@ids)";
        var result = await connect.QueryAsync<OptionsConfig>(sql, new { ids });
        return result.ToList();
    }
}

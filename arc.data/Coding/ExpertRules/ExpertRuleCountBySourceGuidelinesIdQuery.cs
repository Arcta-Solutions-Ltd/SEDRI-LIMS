using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Coding;

/// <summary>
/// Counts expert rules that reference a specification whose guidelinesid matches the given source (listitem) id.
/// Used when deleting a source (guidelines list item) to ensure no expert rules depend on it.
/// </summary>
internal class ExpertRuleCountBySourceGuidelinesIdQuery : IQueryReturningInteger
{
    /// <summary>
    /// Executes the count query. Parameters must contain "sourceid" (the listitem/guidelines id).
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Must contain "sourceid" with the guidelines listitem id.</param>
    /// <returns>Count of expert rules that reference specifications with that guidelinesid.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var sourceId = queryFilters.Parameters.First(p => p.Key.Equals("sourceid", System.StringComparison.OrdinalIgnoreCase)).Value;

        var sql = """
            select count(*) from expertrule e
            inner join specification s on e.specificationid = s.id
            where s.guidelinesid = @SourceId
            """;

        return await connect.QueryFirstAsync<int>(sql, new { SourceId = int.Parse(sourceId) });
    }
}

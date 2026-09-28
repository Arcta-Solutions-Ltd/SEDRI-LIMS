using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specification;

/// <summary>
/// Counts specifications that reference the given version number (listitem) id.
/// Used when deleting a version to ensure no specifications depend on it.
/// </summary>
internal class SpecificationCountByVersionNumberIdQuery : IQueryReturningInteger
{
    /// <summary>
    /// Executes the count query. Parameters must contain "versionnumberid" (the listitem id).
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Must contain "versionnumberid" with the version listitem id.</param>
    /// <returns>Count of specifications that reference that versionnumberid.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var versionNumberId = queryFilters.Parameters.First(p => p.Key.Equals("versionnumberid", System.StringComparison.OrdinalIgnoreCase)).Value;

        var sql = """
            select count(*) from specification s
            where s.versionnumberid = @VersionNumberId
            """;

        return await connect.QueryFirstAsync<int>(sql, new { VersionNumberId = int.Parse(versionNumberId) });
    }
}

using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specification;

/// <summary>
/// Counts specifications that reference the given publication year (listitem) id.
/// Used when deleting a publication year to ensure no specifications depend on it.
/// </summary>
internal class SpecificationCountByPublicationYearIdQuery : IQueryReturningInteger
{
    /// <summary>
    /// Executes the count query. Parameters must contain "publicationyearid" (the listitem id).
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Must contain "publicationyearid" with the publication year listitem id.</param>
    /// <returns>Count of specifications that reference that publicationyearid.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var publicationYearId = queryFilters.Parameters.First(p => p.Key.Equals("publicationyearid", System.StringComparison.OrdinalIgnoreCase)).Value;

        var sql = """
            select count(*) from specification s
            where s.publicationyearid = @PublicationYearId
            """;

        return await connect.QueryFirstAsync<int>(sql, new { PublicationYearId = int.Parse(publicationYearId) });
    }
}

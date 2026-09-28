using arc.domain.Configuration.QueryFiltersConfig;
using Npgsql;
using Dapper;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specification;

/// <summary>
/// Data query that counts how many expert rules, breakpoints, and alerts reference a given specification.
/// Used to prevent deletion of specifications that are in use.
/// </summary>
internal class SpecificationUseCountQuery : IQueryReturningInteger
{
    /// <summary>
    /// Executes the count query.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Must contain "id" with the specification ID.</param>
    /// <returns>The count of expert rules and breakpoints that reference the specification.</returns>
    async Task<int> IQueryReturningInteger.ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var specificationId = queryFilters.Parameters.First(p => p.Key.ToLower() == "id");

        var sql = @"select (select count(*) from expertrule where specificationid = @SpecificationId)
                    + (select count(*) from breakpoint where specificationid = @SpecificationId)
                    + (select count(*) from alert where specificationid = @SpecificationId)";

        var result = await connect.QueryFirstAsync<int>(sql, new { SpecificationId = int.Parse(specificationId.Value) });
        return result;
    }
}

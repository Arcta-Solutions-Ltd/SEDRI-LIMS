using arc.common.ExtensionMethods;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Organisation;

/// <summary>
/// Returns whether an organisation exists and is enabled.
/// </summary>
internal class IsOrganisationEnabledByIdQuery : IQueryReturningInteger
{
    /// <summary>
    /// Executes the query to count enabled organisations matching the supplied id.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="queryFilters">Must contain an <c>id</c> parameter with the organisation id.</param>
    /// <returns>1 when the organisation is enabled; otherwise 0.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var id = queryFilters.Parameters.First(p => p.Key.Equals("id", System.StringComparison.OrdinalIgnoreCase)).Value;
        var sql = $@"select count(id) from organisation where id = @Id and {OrganisationEnabledExtensions.EnabledOnlyPredicate()}";
        return await connect.QueryFirstAsync<int>(sql, new { Id = int.Parse(id) });
    }
}

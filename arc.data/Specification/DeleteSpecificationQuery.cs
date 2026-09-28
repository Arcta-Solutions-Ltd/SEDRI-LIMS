using arc.common.Models.Coding;
using arc.domain.Configuration.QueryFiltersConfig;
using Npgsql;
using Dapper;
using System.Threading.Tasks;

namespace arc.data.Specification;

/// <summary>
/// Data query that loads a single specification by ID for the delete specification flow.
/// Returns the specification data before deletion for confirmation display.
/// </summary>
internal class DeleteSpecificationQuery : IQueryReturningType<SpecificationListModel>
{
    /// <summary>
    /// Executes the query and returns the specification with joined listitem display values.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Must contain "id" with the specification ID.</param>
    /// <returns>The specification row mapped to <see cref="SpecificationListModel"/>.</returns>
    public async Task<SpecificationListModel> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var specificationId = queryFilters.GetIntegerValue("id");
        var sql = "select s.id, guidelinesid, documentid, versionnumberid, publicationyearid, l1.value as guidelines, l2.value as document, l3.value as versionnumber, l4.value as publicationyear from specification s left outer join listitem l1 on l1.id = guidelinesid left outer join listitem l2 on l2.id = documentid left outer join listitem l3 on l3.id = versionnumberid left outer join listitem l4 on l4.id = publicationyearid where s.id = @specificationId;";

        var result = await connect.QueryFirstAsync<SpecificationListModel>(sql, new { specificationId });

        return result;
    }
}

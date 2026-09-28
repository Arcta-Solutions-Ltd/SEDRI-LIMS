using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Specification;

/// <summary>
/// Counts specifications that reference the given document type (listitem) id.
/// Used when deleting a document type to ensure no specifications depend on it.
/// </summary>
internal class SpecificationCountByDocumentIdQuery : IQueryReturningInteger
{
    /// <summary>
    /// Executes the count query. Parameters must contain "documentid" (the listitem id).
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Must contain "documentid" with the document type listitem id.</param>
    /// <returns>Count of specifications that reference that documentid.</returns>
    public async Task<int> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var documentId = queryFilters.Parameters.First(p => p.Key.Equals("documentid", System.StringComparison.OrdinalIgnoreCase)).Value;

        var sql = """
            select count(*) from specification s
            where s.documentid = @DocumentId
            """;

        return await connect.QueryFirstAsync<int>(sql, new { DocumentId = int.Parse(documentId) });
    }
}

using arc.domain.Configuration.QueryFiltersConfig;
using Npgsql;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace arc.data.Specification;

/// <summary>
/// Data query that counts specifications matching the given guidelines, document, version number, and publication year.
/// Used by the specificationexists validation to prevent duplicate specifications.
/// </summary>
internal class SpecificationCountQuery : IQueryReturningInteger
{
    /// <summary>
    /// Executes the count query. Parameters: guidelinesid (required), documentid (required), versionnumberid (optional), publicationyearid (optional).
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Must contain guidelinesid and documentid; versionnumberid and publicationyearid are optional.</param>
    /// <returns>The count of matching specifications.</returns>
    async Task<int> IQueryReturningInteger.ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var guidelinesId = int.Parse(queryFilters.Parameters.First(p => p.Key.Equals("guidelinesid", StringComparison.OrdinalIgnoreCase)).Value);

        var documentId = int.Parse(queryFilters.Parameters.First(p => p.Key.Equals("documentid", StringComparison.OrdinalIgnoreCase)).Value);

        int? versionNumberId = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("versionnumberid", StringComparison.OrdinalIgnoreCase))?.Value is string v ? int.Parse(v) : (int?)null;

        int? publicationYearId = queryFilters.Parameters.FirstOrDefault(p => p.Key.Equals("publicationyearid", StringComparison.OrdinalIgnoreCase))?.Value is string y ? int.Parse(y) : (int?)null;

        var sql = @"select count(s.Id) from specification s where GuidelinesId = @GuidelinesId and DocumentId = @DocumentId and ((@VersionNumberId is null and VersionNumberId is null) or VersionNumberId = @VersionNumberId) and ((@PublicationYearId is null and PublicationYearId is null) or PublicationYearId = @PublicationYearId);";

        return await connect.QueryFirstAsync<int>(sql, new { GuidelinesId = guidelinesId, DocumentId = documentId, VersionNumberId = versionNumberId, PublicationYearId = publicationYearId });
    }
}

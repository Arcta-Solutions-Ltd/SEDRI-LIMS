using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Asset;

/// <summary>
/// Represents a query for retrieving a list of storage options.
/// </summary>
internal class GetStorageForListQuery : IQueryReturningType<List<OptionsConfig>>
{
    /// <summary>
    /// Executes the query asynchronously to retrieve storage options for a list.
    /// </summary>
    /// <param name="connect">The database connection.</param>
    /// <param name="queryFilters">The query filters to apply (not used in this implementation).</param>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result contains a list of storage options.
    /// </returns>
    public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var laboratoryId = queryFilters.GetStringValue("laboratoryid");

        var whereClause = string.IsNullOrEmpty(laboratoryId)
            ? string.Empty
            : "WHERE LaboratoryId = @LaboratoryId";

        var sql = @$"SELECT 
                    id AS key, 
                    fullyqualifiedname || COALESCE(' (' || code || ')', '') AS text, 
                    parentstorageid AS ParentKey
                 FROM storage 
                 {whereClause}
                 ORDER BY fullyqualifiedname";

        var parameters = string.IsNullOrEmpty(laboratoryId)
            ? null
            : new { LaboratoryId = int.Parse(laboratoryId) };

        var result = await connect.QueryAsync<OptionsConfig>(sql, parameters);
        return result.ToList();
    }
}


using arc.domain.Configuration.ListsConfig;
using arc.domain.Configuration.QueryFiltersConfig;
using Dapper;
using Npgsql;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace arc.data.Exports;

/// <summary>
/// Data query that returns export profile options for dropdown/filter lists.
/// Each option has Key (export profile id) and Text (profile name).
/// </summary>
internal class ExportProfileOptionsForListQuery : IQueryReturningType<List<OptionsConfig>>
{
    /// <summary>
    /// Executes the query and returns export profile options for use in filters and dropdowns.
    /// </summary>
    /// <param name="connect">Active database connection.</param>
    /// <param name="queryFilters">Optional filters (unused for options list).</param>
    /// <returns>List of <see cref="OptionsConfig"/> with Key = export profile id, Text = profile name.</returns>
    public async Task<List<OptionsConfig>> ExecuteAsync(NpgsqlConnection connect, QueryFilterConfig queryFilters)
    {
        var sql = @"
            SELECT id::text AS key, name AS text
            FROM exportprofile
            ORDER BY name";

        var result = await connect.QueryAsync<OptionsConfig>(sql);
        return result.ToList();
    }
}
